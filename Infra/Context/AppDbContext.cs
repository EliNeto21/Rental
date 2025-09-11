using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infra.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Motorcycle> Motorcycles => Set<Motorcycle>();
        public DbSet<Courier> Couriers => Set<Courier>();
        public DbSet<Rental> Rentals => Set<Rental>();
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            b.Entity<Motorcycle>(e => {
                e.ToTable("motorcycles");
                e.HasKey(x => x.Id);
                e.Property(x => x.Plate).IsRequired().HasMaxLength(16);
                e.HasIndex(x => x.Plate).IsUnique();
            });
            b.Entity<Courier>(e => {
                e.ToTable("couriers");
                e.HasIndex(x => x.Cnpj).IsUnique();
                e.HasIndex(x => x.CnhNumber).IsUnique();
                e.Property(x => x.CnhType).HasMaxLength(3);
                e.Property(x => x.BirthDate).HasColumnType("date");
            });
            b.Entity<Rental>(e => {
                e.ToTable("rentals");
                e.Property(x => x.StartDate).HasColumnType("date");
                e.Property(x => x.ExpectedEndDate).HasColumnType("date");
                e.Property(x => x.EndDate).HasColumnType("date");
            });
        }
    }
}
