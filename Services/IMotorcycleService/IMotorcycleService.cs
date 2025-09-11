using Domain.Entities;
using Domain.ViewModel;

namespace Services.IMotorcycleService
{
    public interface IMotorcycleService
    {
        Task<GenericResult<Motorcycle>> CreateAsync(MotorcycleViewModel motorcycleViewModel, CancellationToken ct);
        Task<GenericResult<IReadOnlyList<Motorcycle>>> GetByPlateAsync(string? plate, CancellationToken ct);
        Task<GenericResult<Motorcycle>> GetByIdAsync(Guid id, CancellationToken ct);
        Task<GenericResult<dynamic>> UpdatePlateAsync(Guid id, string newPlate, CancellationToken ct);
        Task<GenericResult<dynamic>> DeleteAsync(Guid id, CancellationToken ct);
    }
}
