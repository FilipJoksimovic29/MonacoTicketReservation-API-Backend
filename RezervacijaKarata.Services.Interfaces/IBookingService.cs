using Microsoft.AspNetCore.Mvc;
using RezervacijaKarata.Domain.Model;

namespace RezervacijaKarata.Services.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<Booking> GetBookingByIdAsync(int id);
        Task<Booking> CreateBookingAsync(Booking booking);
   
        Task<bool> DeleteBookingAsync(int id);
        Task<(Booking, bool)> CalculatePriceAndDiscountAsync(Booking booking); 
        Task<Booking> GetBookingByTokenAsync(string token, string email);
        Task<bool> DeleteBookingByTokenAsync(string token, string email);

        Task<bool> UpdateBookingAsync(Booking booking, string token, string email); 

    }
}
