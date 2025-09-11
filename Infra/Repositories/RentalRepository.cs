using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infra.Context;
using Infra.Contracts;

namespace Infra.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly AppDbContext _db;
        public RentalRepository(AppDbContext db) => _db = db;

        public async Task AddAsync(Rental entity, CancellationToken ct)
        {
            _db.Rentals.Add(entity);
            await _db.SaveChangesAsync(ct);
        }

        public Task<bool> AnyByMotorcycleAsync(Guid motorcycleId, CancellationToken ct)
        {
            return _db.Rentals.AnyAsync(r => r.MotorcycleId == motorcycleId, ct);
        }

        public async Task<bool> ExistsOverlappingAsync(
            Guid motorcycleId,
            DateOnly startDate,
            DateOnly expectedEndDate,
            CancellationToken ct)
        {
            var start = startDate.ToDateTime(TimeOnly.MinValue);
            var end = expectedEndDate.ToDateTime(TimeOnly.MinValue);

            var result = _db.Rentals.Where(r => 
                r.MotorcycleId == motorcycleId &&
                r.Status != "Closed" &&
                (r.EndDate == null 
                    ? r.StartDate.ToDateTime(TimeOnly.MinValue) <= end &&
                      r.ExpectedEndDate.ToDateTime(TimeOnly.MinValue) >= start
                    : r.StartDate.ToDateTime(TimeOnly.MinValue) <= end &&
                      r.EndDate.Value.ToDateTime(TimeOnly.MinValue) >= start)
                ).FirstOrDefault();

            return (result != null);
        }

        public Task<Rental?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return _db.Rentals.FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<bool> HasActiveRentalForCourierAsync(Guid courierId, CancellationToken ct)
        {
            return _db.Rentals.AnyAsync(r => r.CourierId == courierId && 
                (r.Status == "Created" || r.Status == "Active"), ct);
        }

        public Task SaveAsync(CancellationToken ct)
        {
            return _db.SaveChangesAsync(ct);
        }
    }
}
