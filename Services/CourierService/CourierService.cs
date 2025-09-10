using Domain.Entities;
using Domain.ViewModel;
using Infra.Contracts;

namespace Services.CourierService
{
    public class CourierService : ICourierService.ICourierService
    {
        private readonly ICourierRepository _repository;

        public CourierService(ICourierRepository repository)
        {
            _repository = repository;
        }

        public async Task<GenericResult<Courier>> RegisterAsync(Courier courier, CancellationToken ct)
        {
            try
            {
                if (!new[] { "A", "B", "A+B" }.Contains(courier.CnhType))
                {
                    throw new InvalidOperationException("Invalid CNH type");
                }

                if (await _repository.CnpjExistsAsync(courier.Cnpj, ct))
                {
                    throw new InvalidOperationException("CNPJ already exists");
                }

                if (await _repository.CnhExistsAsync(courier.CnhNumber, ct))
                {
                    throw new InvalidOperationException("CNH already exists");
                }

                await _repository.AddAsync(courier, ct);

                return new GenericResult<Courier>(200, "", courier);
            }
            catch (Exception ex)
            {
                return new GenericResult<Courier>(400, "Dados inválidos", null);
            }
        }

        public async Task<GenericResult<dynamic>> UpdateCnhImageAsync(Guid courierId, Stream file, string contentType, CancellationToken ct)
        {
            try
            {
                var c = await _repository.GetByIdAsync(courierId, ct) ?? throw new KeyNotFoundException();
                //var url = await _storage.UploadAsync(courierId, file, contentType, ct);
                //c.SetCnhImage(url);
                await _repository.SaveAsync(ct);

                return new GenericResult<dynamic>(200, "", null);
            }
            catch (Exception ex)
            {
                return new GenericResult<dynamic>(400, "Dados inválidos", null);
            }

            
        }
    }
}
