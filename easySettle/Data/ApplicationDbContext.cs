using Microsoft.EntityFrameworkCore;
using easySettle.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace easySettle.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        private readonly IConfiguration _configuration;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<PropertyType> PropertyType { get; set; }
        public DbSet<Amenities> Amenities { get; set; }
        public DbSet<Properties> Properties { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<BuildingAmenities> BuildingAmenities { get; set; }
        public DbSet<ExternalUser> ExternalUser { get; set; }
        public DbSet<PropertyAmenity> PropertyAmenity { get; set; }
        public DbSet<PropertyBuildingAmenity> PropertyBuildingAmenity { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PropertyAmenity>()
                .HasOne(pa => pa.Amenity)
                .WithMany(a => a.PropertyAmenity)
                .HasForeignKey(pa => pa.AmenityId);

            modelBuilder.Entity<BuildingAmenities>()
                .HasMany(b => b.PropertyBuildingAmenity)
                .WithOne(p => p.BuildingAmenity)
                .OnDelete(DeleteBehavior.Cascade);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
