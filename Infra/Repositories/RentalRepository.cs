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

        public Task<bool> ExistsOverlappingAsync(
            Guid motorcycleId,
            DateOnly startDate,
            DateOnly expectedEndDate,
            CancellationToken ct)
        {
            // Janela [StartDate, EndDate] sobrepõe se: r.Start <= expectedEnd && (r.End ?? r.ExpectedEnd) >= start
            var start = startDate.ToDateTime(TimeOnly.MinValue);
            var end = expectedEndDate.ToDateTime(TimeOnly.MinValue);

            return _db.Rentals.AnyAsync(r =>
                r.MotorcycleId == motorcycleId &&
                // Considera locações ainda não finalizadas como até ExpectedEndDate
                (r.EndDate == null
                    ? r.StartDate.ToDateTime(TimeOnly.MinValue) <= end &&
                      r.ExpectedEndDate.ToDateTime(TimeOnly.MinValue) >= start
                    : r.StartDate.ToDateTime(TimeOnly.MinValue) <= end &&
                      r.EndDate.Value.ToDateTime(TimeOnly.MinValue) >= start),
                ct);
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
