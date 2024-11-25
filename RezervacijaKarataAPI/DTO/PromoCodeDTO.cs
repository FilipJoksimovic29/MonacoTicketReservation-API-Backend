namespace RezervacijaKarataAPI.DTO
{
    using RezervacijaKarata.Domain.Model;

    namespace RezervacijaKarataAPI.DTO
    {
        public class PromoCodeDTO
        {
            public string Code { get; set; }
            public int BookingId { get; set; }
            public bool IsUsed { get; set; }
            public int? UsedByBookingId { get; set; }
        }

        public static class PromoCodeDTOMapper
        {
            public static PromoCode ToPromoCode(PromoCodeDTO dto)
            {
                return new PromoCode
                {
                    Code = dto.Code,
                    BookingId = dto.BookingId,
                    IsUsed = dto.IsUsed,
                    UsedByBookingId = dto.UsedByBookingId
                };
            }

            public static PromoCodeDTO ToDTO(PromoCode promoCode)
            {
                return new PromoCodeDTO
                {
                    Code = promoCode.Code,
                    BookingId = promoCode.BookingId,
                    IsUsed = promoCode.IsUsed,
                    UsedByBookingId = promoCode.UsedByBookingId
                };
            }
        }
    }

}
