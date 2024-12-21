using Microsoft.EntityFrameworkCore;
using GharBhada.Models;

namespace GharBhada.Data
{
    public class GharBhadaContext : DbContext
    {
        public GharBhadaContext(DbContextOptions<GharBhadaContext> options) : base(options) { }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }
        public DbSet<Agreement> Agreements { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Favourite> Favourites { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define primary key for PropertyImage and map to table "property_image"
            modelBuilder.Entity<PropertyImage>()
                .ToTable("propertyimage")
                .HasKey(pi => pi.ImageId);

            // Ensure User table is mapped correctly
            modelBuilder.Entity<User>().ToTable("user");

            // Define primary key for User
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);

            // Define primary key for Admin and map to table "admin"
            modelBuilder.Entity<Admin>()
                .ToTable("admin")
                .HasKey(a => a.AdminId);

            // Define primary key for Property and map to table "property"
            modelBuilder.Entity<Property>()
                .ToTable("property")
                .HasKey(p => p.PropertyId);

            // Define primary key for Agreement and map to table "agreement"
            modelBuilder.Entity<Agreement>()
                .ToTable("agreement")
                .HasKey(a => a.AgreementId);

            // Define primary key for Rental and map to table "rental"
            modelBuilder.Entity<Rental>()
                .ToTable("rental")
                .HasKey(r => r.RentalId);

            // Define primary key for Payment and map to table "payment"
            modelBuilder.Entity<Payment>()
                .ToTable("payment")
                .HasKey(p => p.PaymentId);

            // Define primary key for Favourite and map to table "favourite"
            modelBuilder.Entity<Favourite>()
                .ToTable("favourite")
                .HasKey(f => f.FavouriteId);
        }
    }
}