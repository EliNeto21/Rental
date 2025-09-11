using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ViewModel
{
    public class CourierViewModel
    {
        public string Name { get; set; }
        public string Cnpj { get; set; }
        public DateOnly BirthDate { get; set; }
        public string CnhNumber { get; set; }
        public string CnhType { get; set; }
        public string? CnhImageUrl { get; set; }
    }
}
