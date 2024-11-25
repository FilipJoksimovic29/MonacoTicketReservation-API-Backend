using RezervacijaKarata.Domain.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace RezervacijaKarataAPI.DTO
{
    public class BookingDTO
    {
        public int CustomerId { get; set; }

       
        public string BookingStatus { get; set; }

      
        public string? Token { get; set; }

        public bool? PromoCodeUsed { get; set; }

        public decimal? OriginalPrice { get; set; }

        public decimal? FinalPrice { get; set; }

        public bool? IsEarlyBird { get; set; }

        public List<BookingDayZoneDTO> BookingDayZones { get; set; }

       
        public string? GeneratedPromoCode { get; set; }
    }

    public static class BookingDTOMapper
    {
        public static Booking ToBooking(BookingDTO dto)
        {
            return new Booking
            {
                CustomerId = dto.CustomerId,
                BookingStatus = dto.BookingStatus,
                PromoCodeUsed = dto.PromoCodeUsed ?? false, // Use false as default if null
                OriginalPrice = dto.OriginalPrice ?? 0, // Use 0 as default if null
                FinalPrice = dto.FinalPrice ?? 0, // Use 0 as default if null
                IsEarlyBird = dto.IsEarlyBird ?? false, // Use false as default if null
                BookingDayZones = dto.BookingDayZones.Select(bdz => ToBookingDayZone(bdz)).ToList(),
                GeneratedPromoCode = dto.GeneratedPromoCode, // Ensure this is mapped correctly
                Token = dto.Token  // Dodavanje Token-a u mapping


            };
        }

        public static BookingDTO ToDTO(Booking booking)
        {
            return new BookingDTO
            {
                CustomerId = booking.CustomerId,
                Token = booking.Token,  // Uključi Token u odgovor
                PromoCodeUsed = booking.PromoCodeUsed,
                BookingStatus = booking.BookingStatus,
                OriginalPrice = booking.OriginalPrice,
                FinalPrice = booking.FinalPrice,
                IsEarlyBird = booking.IsEarlyBird,
                BookingDayZones = booking.BookingDayZones?.Select(bdz => ToDTO(bdz)).ToList(),
                GeneratedPromoCode = booking.GeneratedPromoCode  // Uključi GeneratedPromoCode u odgovor
            };
        }

        public static BookingDayZone ToBookingDayZone(BookingDayZoneDTO dto)
        {
            return new BookingDayZone
            {
                RaceDayId = dto.RaceDayId,
                SeatingZoneId = dto.SeatingZoneId
            };
        }

        public static BookingDayZoneDTO ToDTO(BookingDayZone bdz)
        {
            return new BookingDayZoneDTO
            {
                RaceDayId = bdz.RaceDayId,
                SeatingZoneId = bdz.SeatingZoneId
            };
        }
    }
}
