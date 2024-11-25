using Microsoft.EntityFrameworkCore;
using RezervacijaKarata.DataAccess;
using RezervacijaKarata.Domain.Model;
using RezervacijaKarata.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezervacijaKarata.Services
{
    public class SeatingZoneService : ISeatingZoneService
    {
        private readonly DataContext _context;

        public SeatingZoneService(DataContext context)
        {
            _context = context;
        }

        //public async Task<IEnumerable<SeatingZone>> GetAllSeatingZonesAsync()
        //{
        //    return await _context.SeatingZones.ToListAsync();
        //}

        public async Task<SeatingZone> GetSeatingZoneByIdAsync(int id)
        {
            return await _context.SeatingZones.FindAsync(id);
        }

        public async Task<IEnumerable<SeatingZone>> GetAllSeatingZonesAsync(int? capacity, decimal? price, bool? isAccessible, bool? hasLargeTV, string name = null)
        {
            var query = _context.SeatingZones.AsQueryable();

            if (capacity.HasValue)
                query = query.Where(sz => sz.Capacity == capacity.Value);
            if (price.HasValue)
                query = query.Where(sz => sz.Price <= price.Value);
            if (isAccessible.HasValue)
                query = query.Where(sz => sz.IsAccessible == isAccessible.Value);
            if (hasLargeTV.HasValue)
                query = query.Where(sz => sz.HasLargeTV == hasLargeTV.Value);
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(sz => EF.Functions.Like(sz.Name, $"%{name}%"));

            return await query.ToListAsync();
        }

        public async Task<SeatingZone> CreateSeatingZoneAsync(SeatingZone seatingZone)
        {
            _context.SeatingZones.Add(seatingZone);
            await _context.SaveChangesAsync();
            return seatingZone;
        }

        public async Task UpdateSeatingZoneAsync(SeatingZone seatingZone)
        {
            _context.Entry(seatingZone).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSeatingZoneAsync(int id)
        {
            var seatingZone = await _context.SeatingZones.FindAsync(id);
            if (seatingZone != null)
            {
                _context.SeatingZones.Remove(seatingZone);
                await _context.SaveChangesAsync();
            }
        }
    }

}
