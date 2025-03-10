using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using GharBhada.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection.Extensions;
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

// Add services to the container.
builder.Services.AddControllers();

// Configure AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Register AutoMapper and add the profile
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Configure DbContext with MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<GharBhadaContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Configure CORS

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


// **JWT Authentication Config**
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

// Firebase Initialization (using the service account key path from appsettings.json)
var firebaseCredentialsPath = builder.Configuration["Firebase:CredentialsPath"];
if (string.IsNullOrEmpty(firebaseCredentialsPath))
{
    throw new InvalidOperationException("Firebase credentials path is not configured.");
}

FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile(firebaseCredentialsPath)
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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<JwtTokenService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseCors("AllowAll");


// **Enable Authentication and Authorization**
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