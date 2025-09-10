using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infra.Contracts
{
    public interface IRentalRepository
    {
        Task AddAsync(Rental entity, CancellationToken ct);
        Task<Rental?> GetByIdAsync(Guid id, CancellationToken ct);
        Task SaveAsync(CancellationToken ct);

        // disponibilidade da moto (período não pode sobrepor)
        Task<bool> ExistsOverlappingAsync(
            Guid motorcycleId,
            DateOnly startDate,
            DateOnly expectedEndDate,
            CancellationToken ct);

        // opcional: não permitir múltiplas locações ativas do mesmo entregador
        Task<bool> HasActiveRentalForCourierAsync(Guid courierId, CancellationToken ct);

        // usado no delete da moto, se optarmos por bloquear qualquer histórico
        Task<bool> AnyByMotorcycleAsync(Guid motorcycleId, CancellationToken ct);
    }
}
