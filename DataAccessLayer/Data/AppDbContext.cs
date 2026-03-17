using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;


namespace DataAccessLayer.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ServiceCategory> ServiceCategories { get; set; }

        public DbSet<Booking> Bookings { get; set; }                 
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .Property(u => u.HourlyRate)
                .HasColumnType("decimal(18,2)");

            // Booking -> Customer (ApplicationUser)
            builder.Entity<Booking>()
                .HasOne(b => b.Customer)
                .WithMany()                           // you can add .WithMany(u => u.CustomerBookings) later
                .HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Booking -> Provider (ApplicationUser)
            builder.Entity<Booking>()
                .HasOne(b => b.Provider)
                .WithMany()                           // you can add .WithMany(u => u.ProviderBookings) later
                .HasForeignKey(b => b.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Booking>()
                .Property(b => b.Status)
                .HasMaxLength(20)
                .IsRequired();

            // Review relations
            builder.Entity<Review>()
                .HasOne(r => r.Booking)
                .WithMany()
                .HasForeignKey(r => r.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Review>()
                .HasOne(r => r.Customer)
                .WithMany()
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Review>()
                .HasOne(r => r.Provider)
                .WithMany()
                .HasForeignKey(r => r.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
