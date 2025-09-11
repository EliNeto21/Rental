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

        public async Task<GenericResult<Rental>> CreateAsync(RentalViewModel rentalViewModel, CancellationToken ct)
        {
            try
            {
                if (rentalViewModel.StartDate.AddDays(1) < DateOnly.FromDateTime(DateTime.Now))
                {
                    throw new Exception("The start date cannot be in the past.");
                }

                if (rentalViewModel.PlanDays is not (7 or 15 or 30 or 45 or 50))
                {
                    throw new Exception("Invalid rental plan. Allowed values: 7, 15, 30, 45, 50");
                }

                var courier = await _couriers.GetByIdAsync(rentalViewModel.CourierId, ct);

                if (courier == null)
                {
                    throw new Exception("Courier not found");
                }

                if (courier.CnhImageUrl == null)
                {
                    throw new Exception("Courier CNH image not uploaded");
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

                if (rental.DailyRate <= 0)
                {
                    throw new ArgumentException("Daily rate must be greater than zero");
                }

                await _repository.AddAsync(rental, ct);

                return new GenericResult<Rental>(200, "Rental completed successfully", rental);
            }
            catch (Exception ex)
            {
                return new GenericResult<Rental>(400, ex.Message, null);
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
                return new GenericResult<Rental>(400, ex.Message, null);
            }
        }

        public async Task<GenericResult<ReturnRentalResponse>> ReturnRentalAsync(Guid rentalId, DateOnly endDate, CancellationToken ct)
        {
            try
            {
                var rental = await _repository.GetByIdAsync(rentalId, ct);

                if (rental == null)
                {
                    throw new Exception("Rental not found");
                }

                if (endDate < rental.StartDate)
                {
                    throw new Exception("End date cannot be before start date");
                }

                if (rental.Status == "Closed")
                {
                    throw new Exception("Rental is closed");
                }

                var result = rental.Close(endDate);

                await _repository.SaveAsync(ct);

                return new GenericResult<ReturnRentalResponse>(200, "Success", result);
            }
            catch (Exception ex)
            {
                return new GenericResult<ReturnRentalResponse>(400, ex.Message, null);
            }
        }
    }
}
