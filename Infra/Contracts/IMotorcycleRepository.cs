using Domain.Entities;

namespace Infra.Contracts
{
    public interface IMotorcycleRepository
    {
        Task<bool> PlateExistsAsync(string plate, CancellationToken ct);
        Task AddAsync(Motorcycle entity, CancellationToken ct);
        Task<Motorcycle?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<IReadOnlyList<Motorcycle>> SearchAsync(string? plate, CancellationToken ct);
        Task UpdatePlateAsync(Guid id, string newPlate, CancellationToken ct);
        Task<bool> HasAnyRentalAsync(Guid motorcycleId, CancellationToken ct);
        Task DeleteAsync(Guid id, CancellationToken ct);
    }
}
