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

        }
    }
    

}

