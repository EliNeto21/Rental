using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.ViewModel;
using Infra.Contracts;
using static Domain.Entities.Rental;

namespace Services.RentalService
{
    public class RentalService : IRentalService.IRentalService
    {
        private readonly IRentalRepository _repository;
        private readonly ICourierRepository _couriers;
        private readonly IMotorcycleRepository _motorcycle;

        public RentalService(
            IRentalRepository repo, 
            ICourierRepository couriers, 
            IMotorcycleRepository motos)
        { 
            _repository = repo; 
            _couriers = couriers; 
            _motorcycle = motos; 
        }

        public async Task<GenericResult<RentalViewModel>> CreateAsync(RentalViewModel rentalViewModel, CancellationToken ct)
        {
            try
            {
                var courier = await _couriers.GetByIdAsync(rentalViewModel.CourierId, ct);

                if (courier == null)
                {
                    throw new Exception("Courier not found");
                }

                if (!courier.CanRentMotorcycle())
                {
                    throw new Exception("Courier not eligible (CNH A required)");
                }

                var motorcycle = await _motorcycle.GetByIdAsync(rentalViewModel.MotorcycleId, ct);

                if (motorcycle == null)
                {
                    throw new Exception("Motorcycle not found");
                }

                var startDate = rentalViewModel.StartDate.AddDays(1);
                var expectedEndDate = startDate.AddDays(rentalViewModel.PlanDays);

                var overlapping = await _repository.ExistsOverlappingAsync(rentalViewModel.MotorcycleId, startDate, expectedEndDate, ct);

                if (overlapping)
                {
                    throw new Exception("Motorcycle not available for the selected period");
                }

                var rental = new Rental(rentalViewModel.MotorcycleId, rentalViewModel.CourierId, rentalViewModel.PlanDays, 0, startDate);
                await _repository.AddAsync(rental, ct);

                var rentalView = new RentalViewModel
                {
                    MotorcycleId = rental.MotorcycleId,
                    CourierId = rental.CourierId,
                    PlanDays = rental.PlanDays,
                    StartDate = rental.StartDate,
                    ExpectedEndDate = rental.ExpectedEndDate
                };

                return new GenericResult<RentalViewModel>(200, "Rental completed successfully", rentalView);
            }
            catch
            {
                return new GenericResult<RentalViewModel>(400, "Invalid data", null);
            }
        }

        public async Task<GenericResult<Rental>> GetByIdAsync(Guid rentalId, CancellationToken ct)
        {
            try
            {
                var rental = await _repository.GetByIdAsync(rentalId, ct);

                if (rental == null)
                {
                    throw new Exception("Rental not found");
                }

                return new GenericResult<Rental>(200, "Success", rental);
            }
            catch (Exception ex)
            {
                return new GenericResult<Rental>(400, "Invalid data", null);
            }
        }

        public async Task<GenericResult<ReturnRentalResponse>> ReturnRentalAsync(Guid rentalId, DateOnly endDate, CancellationToken ct)
        {
            try
            {
                var rental = await _repository.GetByIdAsync(rentalId, ct);

                if (rental == null)
                {
                    throw new KeyNotFoundException("Rental not found");
                }

                var result = rental.Close(endDate);

                await _repository.SaveAsync(ct);

                return new GenericResult<ReturnRentalResponse>(200, "Success", result);
            }
            catch
            {
                return new GenericResult<ReturnRentalResponse>(400, "Invalid data", null);
            }
        }
    }
}
