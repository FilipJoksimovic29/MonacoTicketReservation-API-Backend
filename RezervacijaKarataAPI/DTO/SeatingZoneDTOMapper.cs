using RezervacijaKarata.Domain.Model;

namespace RezervacijaKarataAPI.DTO
{
    public class SeatingZoneDTO
    {
        public int Id { get; set; }  // Dodajte ovo
        public string Name { get; set; }
        public int Capacity { get; set; }
        public decimal Price { get; set; }
        public bool IsAccessible { get; set; }
        public bool HasLargeTV { get; set; }
    }

    public static class SeatingZoneDTOMapper
    {
        public static SeatingZone ToSeatingZone(this SeatingZoneDTO dto)
        {
            return new SeatingZone
            {
                Id = dto.Id,  // Dodajte ovo
                Name = dto.Name,
                Capacity = dto.Capacity,
                Price = dto.Price,
                IsAccessible = dto.IsAccessible,
                HasLargeTV = dto.HasLargeTV
            };
        }

        public static SeatingZoneDTO ToDTO(this SeatingZone seatingZone)
        {
            return new SeatingZoneDTO
            {
                Id = seatingZone.Id,  // Dodajte ovo
                Name = seatingZone.Name,
                Capacity = seatingZone.Capacity,
                Price = seatingZone.Price,
                IsAccessible = seatingZone.IsAccessible,
                HasLargeTV = seatingZone.HasLargeTV
            };
        }
    }
}
