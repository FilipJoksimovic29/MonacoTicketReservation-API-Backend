using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezervacijaKarata.Domain.Model
{
    public class SeatingZone
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public decimal Price { get; set; }
        public bool IsAccessible { get; set; }
        public bool HasLargeTV { get; set; }
        
    }
}
