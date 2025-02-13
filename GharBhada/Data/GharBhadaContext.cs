using Microsoft.EntityFrameworkCore;
using GharBhada.Models;

namespace GharBhada.Data
{
    public class GharBhadaContext : DbContext
    {
        public GharBhadaContext(DbContextOptions<GharBhadaContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<Agreement> Agreements { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Favourite> Favourites { get; set; }
        public DbSet<MoveInAssistance> MoveInAssistances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define primary key for PropertyImage and map to table "property_image"
            modelBuilder.Entity<PropertyImage>()
                .ToTable("propertyimage")
                .HasKey(pi => pi.PropertyImageId);

            // Ensure User table is mapped correctly
            modelBuilder.Entity<User>().ToTable("user");

            // Define primary key for User
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);

            // Define primary key for UserDetail and map to table "userdetail"
            modelBuilder.Entity<UserDetail>()
                .ToTable("userdetail")
                .HasKey(ud => ud.UserDetailId);

            // Define primary key for Property and map to table "property"
            modelBuilder.Entity<Property>()
                .ToTable("property")
                .HasKey(p => p.PropertyId);

            // Define primary key for Agreement and map to table "agreement"
            modelBuilder.Entity<Agreement>()
                .ToTable("agreement")
                .HasKey(a => a.AgreementId);

            // Define primary key for Booking and map to table "booking"
            modelBuilder.Entity<Booking>()
                .ToTable("booking")
                .HasKey(b => b.BookingId);

            // Define primary key for Payment and map to table "payment"
            modelBuilder.Entity<Payment>()
                .ToTable("payment")
                .HasKey(p => p.PaymentId);

            // Define primary key for Favourite and map to table "favourite"
            modelBuilder.Entity<Favourite>()
                .ToTable("favourite")
                .HasKey(f => f.FavouriteId);

            // Define primary key for MoveInAssistance and map to table "moveinassistance"
            modelBuilder.Entity<MoveInAssistance>()
                .ToTable("moveinassistance")
                .HasKey(mia => mia.AssistanceId);
        }
    }
}