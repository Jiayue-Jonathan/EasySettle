//AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using EasySettle.Models;

namespace EasySettle.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Property> Properties { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Lease> Leases { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserProperty> UserProperties { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure one-to-many relationship between Owner and Property
            modelBuilder.Entity<Property>()
                .HasOne(p => p.Owner)
                .WithMany(o => o.Properties)
                .HasForeignKey(p => p.OwnerID);

            // Configure one-to-many relationship between Property and Lease
            modelBuilder.Entity<Lease>()
                .HasOne(l => l.Property)
                .WithMany(p => p.Leases)
                .HasForeignKey(l => l.PropertyID);
            
            modelBuilder.Entity<Lease>()
                .HasOne(l => l.Client)
                .WithOne()
                .HasForeignKey<Lease>(l => l.ClientID);

            // Configuration for the many-to-many relationship between User and Property through UserProperty
            modelBuilder.Entity<UserProperty>()
                .HasKey(up => new { up.Email, up.PropertyID }); // Composite key

            modelBuilder.Entity<UserProperty>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserProperties)
                .HasForeignKey(up => up.Email);

            modelBuilder.Entity<UserProperty>()
                .HasOne(up => up.Property)
                .WithMany(p => p.UserProperties)
                .HasForeignKey(up => up.PropertyID);
        }
    }
    

}

