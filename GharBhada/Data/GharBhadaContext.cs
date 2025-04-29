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

            modelBuilder.Entity<PropertyImage>()
                .ToTable("propertyimage")
                .HasKey(pi => pi.PropertyImageId);

            modelBuilder.Entity<User>().ToTable("user");

            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<UserDetail>()
                .ToTable("userdetail")
                .HasKey(ud => ud.UserDetailId);

            modelBuilder.Entity<Property>()
                .ToTable("property")
                .HasKey(p => p.PropertyId);

            modelBuilder.Entity<Agreement>()
                .ToTable("agreement")
                .HasKey(a => a.AgreementId);

            modelBuilder.Entity<Booking>()
                .ToTable("booking")
                .HasKey(b => b.BookingId);

            modelBuilder.Entity<Payment>()
                .ToTable("payment")
                .HasKey(p => p.PaymentId);

            modelBuilder.Entity<Favourite>()
                .ToTable("favourite")
                .HasKey(f => f.FavouriteId);

            modelBuilder.Entity<MoveInAssistance>()
                .ToTable("moveinassistance")
                .HasKey(mia => mia.MoveInAssistanceId);
        }
    }
}