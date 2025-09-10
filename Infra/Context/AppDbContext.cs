using System.Reflection.Emit;
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
                e.Property(x => x.BirthDate).HasConversion(
                    v => v.ToDateTime(TimeOnly.MinValue),
                    v => DateOnly.FromDateTime(v));
            });
            b.Entity<Rental>(e => {
                e.ToTable("rentals");
                e.HasOne<Motorcycle>().WithMany().HasForeignKey(x => x.MotorcycleId);
                e.HasOne<Courier>().WithMany().HasForeignKey(x => x.CourierId);
                e.Property(x => x.StartDate).HasConversion(
                    v => v.ToDateTime(TimeOnly.MinValue),
                    v => DateOnly.FromDateTime(v));
                e.Property(x => x.ExpectedEndDate).HasConversion(
                    v => v.ToDateTime(TimeOnly.MinValue),
                    v => DateOnly.FromDateTime(v));
                e.Property(x => x.EndDate).HasConversion(
                    v => v.HasValue ? v.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                    v => v.HasValue ? DateOnly.FromDateTime(v.Value) : (DateOnly?)null);
            });
        }
    }
}
