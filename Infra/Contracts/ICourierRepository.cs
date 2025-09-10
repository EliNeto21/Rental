using Domain.Entities;

namespace Infra.Contracts
{
    public interface ICourierRepository
    {
        Task<bool> CnpjExistsAsync(string cnpj, CancellationToken ct);
        Task<bool> CnhExistsAsync(string cnhNumber, CancellationToken ct);

        Task AddAsync(Courier entity, CancellationToken ct);
        Task<Courier?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<Courier?> GetByCnpjAsync(string cnpj, CancellationToken ct);
        Task<Courier?> GetByCnhAsync(string cnhNumber, CancellationToken ct);

        Task SaveAsync(CancellationToken ct);
    }
}
