using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezervacijaKarata.Domain.Model
{
    public class Booking
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }

        public Customer Customer { get; set; }
        public string Token { get; set; }
        public bool PromoCodeUsed { get; set; }
        public string BookingStatus { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal FinalPrice { get; set; }
        public bool IsEarlyBird { get; set; }  
        public ICollection<BookingDayZone> BookingDayZones { get; set; }


        [NotMapped] 
        public string GeneratedPromoCode { get; set; }
    }
}
