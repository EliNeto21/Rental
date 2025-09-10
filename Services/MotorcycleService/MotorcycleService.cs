using Domain.ViewModel;
using Domain.Entities;
using Infra.Contracts;

namespace Services.MotorcycleService
{
    public class MotorcycleService : IMotorcycleService.IMotorcycleService
    {
        private readonly IMotorcycleRepository _repository;

        public MotorcycleService(
                IMotorcycleRepository repo) 
        {
            _repository = repo; 
        }

        public async Task<GenericResult<Motorcycle>> CreateAsync(MotorcycleViewModel motorcycleViewModel, CancellationToken ct)
        {
            try
            {
                if (await _repository.PlateExistsAsync(motorcycleViewModel.Plate.Trim().ToUpperInvariant(), ct))
                {
                    throw new InvalidOperationException("Plate already exists");
                }

                var motorcycle = new Motorcycle(motorcycleViewModel.Year, motorcycleViewModel.Model, motorcycleViewModel.Plate);
                await _repository.AddAsync(motorcycle, ct);

                //var @event = new MotorcycleRegisteredEvent(m.Id, m.Year, m.Model, m.Plate, m.CreatedAt);
                //await _bus.PublishAsync("rental.events", "motorcycle.registered", @event, ct);

                return new GenericResult<Motorcycle>(200, "", motorcycle);
            }
            catch (Exception ex)
            {
                return new GenericResult<Motorcycle>(400, "Dados inválidos", null);
            }
        }

        public async Task<GenericResult<IReadOnlyList<Motorcycle>>> GetByPlateAsync(string? plate, CancellationToken ct)
        {
            try
            {
                var result = await _repository.SearchAsync(plate?.Trim().ToUpperInvariant(), ct); 

                if (result == null)
                {
                    throw new Exception("Nenhum registro encontrado");
                }

                return new GenericResult<IReadOnlyList<Motorcycle>>(200, "", result);
            }
            catch (Exception ex)
            {
                return new GenericResult<IReadOnlyList<Motorcycle>>(400, ex.Message, null);
            }
        }

        public async Task<GenericResult<Motorcycle>> GetByIdAsync(Guid id, CancellationToken ct)
        {
            try
            {
                var result = await _repository.GetByIdAsync(id, ct);

                if (result == null)
                {
                    throw new Exception("Nenhum registro encontrado");
                }

                return new GenericResult<Motorcycle>(200, "", result);
            }
            catch (Exception ex)
            {
                return new GenericResult<Motorcycle>(400, ex.Message, null);
            }
        }

        public async Task UpdatePlateAsync(Guid id, string newPlate, CancellationToken ct)
        {
            newPlate = newPlate.Trim().ToUpperInvariant();

            if (await _repository.PlateExistsAsync(newPlate, ct))
            {
                throw new InvalidOperationException("Plate already exists");
            }
                
            await _repository.UpdatePlateAsync(id, newPlate, ct);
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct)
        {
            if (await _repository.HasAnyRentalAsync(id, ct)) 
            {
                throw new InvalidOperationException("Motorcycle has rentals");
            }

            await _repository.DeleteAsync(id, ct);
        }
    }
}
