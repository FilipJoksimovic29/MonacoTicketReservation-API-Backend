using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezervacijaKarata.Domain.Model
{
    public class PromoCode
    {
        public int Id { get; set; } 
        public string Code { get; set; }  
        public int BookingId { get; set; }  
        public bool IsUsed { get; set; }  
        public int? UsedByBookingId { get; set; }  

    }

}
