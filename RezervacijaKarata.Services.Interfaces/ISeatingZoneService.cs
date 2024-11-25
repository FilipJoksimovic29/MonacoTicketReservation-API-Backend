using RezervacijaKarata.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezervacijaKarata.Services.Interfaces
{
    public interface ISeatingZoneService
    {
        Task<IEnumerable<SeatingZone>> GetAllSeatingZonesAsync(int? capacity = null, decimal? price = null, bool? isAccessible = null, bool? hasLargeTV = null, string name = null);
        Task<SeatingZone> GetSeatingZoneByIdAsync(int id);
        Task<SeatingZone> CreateSeatingZoneAsync(SeatingZone seatingZone);
        Task UpdateSeatingZoneAsync(SeatingZone seatingZone);
        Task DeleteSeatingZoneAsync(int id);
    }
}
