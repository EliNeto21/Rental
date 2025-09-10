using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infra.Context;
using Infra.Contracts;

namespace Infra.Repositories
{
    public class MotorcycleRepository : IMotorcycleRepository
    {
        private readonly AppDbContext _db;
        public MotorcycleRepository(AppDbContext db) => _db = db;

        public async Task AddAsync(Motorcycle motorcycle, CancellationToken ct)
        {
            _db.Motorcycles.Add(motorcycle);
            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct)
        {
            var motorcycle = await GetByIdAsync(id, ct) ?? throw new KeyNotFoundException();
            _db.Motorcycles.Remove(motorcycle);
            await _db.SaveChangesAsync(ct);
        }

        public Task<Motorcycle?> GetByIdAsync(Guid id, CancellationToken ct)
            => _db.Motorcycles.FirstOrDefaultAsync(x => x.Id == id, ct);

        public Task<bool> HasAnyRentalAsync(Guid motorcycleId, CancellationToken ct)
            => _db.Rentals.AnyAsync(r => r.MotorcycleId == motorcycleId, ct);

        public Task<bool> PlateExistsAsync(string plate, CancellationToken ct)
            => _db.Motorcycles.AnyAsync(x => x.Plate == plate, ct);

        public async Task<IReadOnlyList<Motorcycle>> SearchAsync(string? plate, CancellationToken ct)
        {
            IQueryable<Motorcycle> q = _db.Motorcycles;
            if (!string.IsNullOrWhiteSpace(plate)) q = q.Where(x => x.Plate.Contains(plate));
            return await q.OrderByDescending(x => x.CreatedAt).ToListAsync(ct);
        }

        public async Task UpdatePlateAsync(Guid id, string newPlate, CancellationToken ct)
        {
            var motorcycle = await GetByIdAsync(id, ct) ?? throw new KeyNotFoundException();
            motorcycle.UpdatePlate(newPlate);
            await _db.SaveChangesAsync(ct);
        }
    }
}
