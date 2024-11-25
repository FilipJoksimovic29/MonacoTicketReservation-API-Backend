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
    public class RaceDayService : IRaceDayService
    {
        private readonly DataContext _context;

        public RaceDayService(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RaceDay>> GetAllRaceDaysAsync()
        {
            return await _context.RaceDays.ToListAsync();
        }

        public async Task<RaceDay> GetRaceDayByIdAsync(int id)
        {
            return await _context.RaceDays.FindAsync(id);
        }

        public async Task<RaceDay> CreateRaceDayAsync(RaceDay raceDay)
        {
            _context.RaceDays.Add(raceDay);
            await _context.SaveChangesAsync();
            return raceDay;
        }

        public async Task UpdateRaceDayAsync(RaceDay raceDay)
        {
            _context.Entry(raceDay).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRaceDayAsync(int id)
        {
            var raceDay = await _context.RaceDays.FindAsync(id);
            if (raceDay != null)
            {
                _context.RaceDays.Remove(raceDay);
                await _context.SaveChangesAsync();
            }
        }
    }

}
