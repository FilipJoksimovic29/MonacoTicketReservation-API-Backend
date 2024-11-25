using RezervacijaKarata.Domain.Model;

namespace RezervacijaKarataAPI.DTO
{
    public class RaceDayDTO
    {
        public DateTime Date { get; set; }
    }

    public static class RaceDayDTOMapper
    {
        public static RaceDay ToRaceDay(this RaceDayDTO dto)
        {
            return new RaceDay
            {
                Date = dto.Date
            };
        }

        public static RaceDayDTO ToDTO(this RaceDay raceDay)
        {
            return new RaceDayDTO
            {
                Date = raceDay.Date
            };
        }
    }
}
