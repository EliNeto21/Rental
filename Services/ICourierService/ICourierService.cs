using Domain.Entities;
using Domain.ViewModel;

namespace Services.ICourierService
{
    public interface ICourierService
    {
        Task<GenericResult<Courier>> RegisterAsync(CourierViewModel courier, CancellationToken ct);
        Task<GenericResult<Courier>> UpdateCnhImageAsync(Guid courierId, Stream file, string contentType, CancellationToken ct);
    }
}
