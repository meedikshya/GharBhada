using GharBhada.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using GharBhada.Repositories.GenericRepositories;
using GharBhada.Repositories.SpecificRepositories.AgreementRepositories;
using GharBhada.Repositories.SpecificRepositories.BookingRepositories;
using GharBhada.Repositories.SpecificRepositories.FavouriteRepositories;
using GharBhada.Repositories.SpecificRepositories.MoveInAssistanceRepositories;
using GharBhada.Repositories.SpecificRepositories.PaymentRepositories;
using GharBhada.Repositories.SpecificRepositories.PropertyImageRepositories;
using GharBhada.Repositories.SpecificRepositories.UserRepositories;
using GharBhada.Repositories.SpecificRepositories.UserDetailRepositories;
using GharBhada.Repositories.SpecificRepositories.PropertyRepositories;
using MySqlConnector;
using System.Text;
using GharBhada.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8000);
});

builder.Services.AddControllers();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAutoMapper(typeof(MappingProfile));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<GharBhadaContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

var jwtSettings = builder.Configuration.GetSection("Jwt");
var keyString = jwtSettings["Key"];

if (string.IsNullOrEmpty(keyString))
{
    throw new InvalidOperationException("JWT Key is not configured.");
}

var key = Encoding.ASCII.GetBytes(keyString);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// Add Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("Renter", policy => policy.RequireRole("Renter"));
    options.AddPolicy("Landlord", policy => policy.RequireRole("Landlord"));
});

// Register IGenericRepositories with its implementation 
builder.Services.AddScoped<IGenericRepositories, GenericRepositories>();
builder.Services.AddScoped<IAgreementRepositories, AgreementRepositories>();
builder.Services.AddScoped<IBookingRepositories, BookingRepositories>();
builder.Services.AddScoped<IFavouriteRepositories, FavouriteRepositories>();
builder.Services.AddScoped<IMoveInAssistanceRepositories, MoveInAssistanceRepositories>();
builder.Services.AddScoped<IPaymentRepositories, PaymentRepositories>();
builder.Services.AddScoped<IPropertyImageRepositories, PropertyImageRepositories>();
builder.Services.AddScoped<IUserRepositories, UserRepositories>();
builder.Services.AddScoped<IUserDetailRepositories, UserDetailRepositories>();
builder.Services.AddScoped<IPropertyRepositories, PropertyRepositories>();

// Register JwtTokenService
builder.Services.AddScoped<JwtTokenService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

// Enable Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<GharBhadaContext>();
    try
    {
        var canConnect = dbContext.Database.CanConnect();
        Console.WriteLine($"Database connection successful: {canConnect}");

        if (canConnect)
        {
            using var connection = new MySqlConnection(connectionString);
            connection.Open();
            using var command = new MySqlCommand("SHOW TABLES;", connection);
            using var reader = command.ExecuteReader();
            Console.WriteLine("Tables in the database:");
            while (reader.Read())
            {
                Console.WriteLine(reader.GetString(0));
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database connection failed: {ex.Message}");
    }
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<GharBhadaContext>();
    dbContext.Database.Migrate();
}

app.Run();