using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infra.Contracts;

namespace Services.RentalService
{
    public class RentalService
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


    }
}
