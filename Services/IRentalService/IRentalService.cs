using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.ViewModel;
using static Domain.Entities.Rental;

namespace Services.IRentalService
{
    public interface IRentalService
    {
        Task<GenericResult<RentalViewModel>> CreateAsync(RentalViewModel rentalViewModel, CancellationToken ct);
        Task<GenericResult<Rental>> GetByIdAsync(Guid rentalId, CancellationToken ct);
        Task<GenericResult<ReturnRentalResponse>> ReturnRentalAsync(Guid rentalId, DateOnly endDate, CancellationToken ct);
    }
}
