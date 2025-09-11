using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class RentalViewModel
    {
        public Guid Id { get; private set; }
        public Guid CourierId { get; set; }
        public Guid MotorcycleId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public DateOnly ExpectedEndDate { get; set; }
        public int PlanDays { get; set; }

    }
}
