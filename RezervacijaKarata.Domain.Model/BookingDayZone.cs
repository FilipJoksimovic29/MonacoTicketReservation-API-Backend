using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezervacijaKarata.Domain.Model
{
    public class BookingDayZone
    {
        public int BookingId { get; set; }
        public Booking Booking { get; set; }

        public int RaceDayId { get; set; }
        public RaceDay RaceDay { get; set; }

        public int SeatingZoneId { get; set; }
        public SeatingZone SeatingZone { get; set; }
    }
}
