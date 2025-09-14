using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Event
{
    public class MotorcycleEvent
    {
        public Guid Id { get; set; }
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Plate { get; set; }
    }
}
