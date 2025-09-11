using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infra.Context;
using Infra.Contracts;

namespace Infra.Repositories
{
    public class CourierRepository : ICourierRepository
    {
        private readonly AppDbContext _db;
        public CourierRepository(AppDbContext db) => _db = db;

        public async Task AddAsync(Courier entity, CancellationToken ct)
        {
            _db.Couriers.Add(entity);
            await _db.SaveChangesAsync(ct);
        }

        public Task<bool> CnhExistsAsync(string cnhNumber, CancellationToken ct)
        {
            return _db.Couriers.AnyAsync(x => x.CnhNumber == cnhNumber, ct);
        }

        public Task<bool> CnpjExistsAsync(string cnpj, CancellationToken ct)
        {
            return _db.Couriers.AnyAsync(x => x.Cnpj == cnpj, ct);
        }

        public Task<Courier?> GetByCnhAsync(string cnhNumber, CancellationToken ct)
        {
            return _db.Couriers.FirstOrDefaultAsync(x => x.CnhNumber == cnhNumber, ct);
        }

        public Task<Courier?> GetByCnpjAsync(string cnpj, CancellationToken ct)
        {
            return _db.Couriers.FirstOrDefaultAsync(x => x.Cnpj == cnpj, ct);
        }

        public Task<Courier?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return _db.Couriers.FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task SaveAsync(Courier entity, CancellationToken ct)
        {
            _db.Couriers.Update(entity);
            return _db.SaveChangesAsync(ct);
        }
    }
}
