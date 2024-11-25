using RezervacijaKarata.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezervacijaKarata.Services.Interfaces
{
    public interface IRaceDayService
    {
        Task<IEnumerable<RaceDay>> GetAllRaceDaysAsync();
        Task<RaceDay> GetRaceDayByIdAsync(int id);
        Task<RaceDay> CreateRaceDayAsync(RaceDay raceDay);
        Task UpdateRaceDayAsync(RaceDay raceDay);
        Task DeleteRaceDayAsync(int id);
    }
}
