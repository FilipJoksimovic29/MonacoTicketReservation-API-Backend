using RezervacijaKarata.Domain.Model;
using RezervacijaKarata.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Security.Cryptography;
using System.Diagnostics;

public class BookingService : IBookingService
{
    private readonly DataContext _context;
    private readonly DateTime earlyBirdCutoff = new DateTime(2025, 11, 1); // Early bird cutoff date

    public BookingService(DataContext context)
    {
        _context = context;
    }


    private async Task<bool> CheckAvailability(Booking booking)
    {
        foreach (var bdz in booking.BookingDayZones)
        {
            var totalBooked = await _context.BookingDayZones
                                            .Where(x => x.RaceDayId == bdz.RaceDayId && x.SeatingZoneId == bdz.SeatingZoneId)
                                            .CountAsync();

            var zoneCapacity = await _context.SeatingZones
                                            .Where(z => z.Id == bdz.SeatingZoneId)
                                            .Select(z => z.Capacity)
                                            .FirstOrDefaultAsync();

            if (totalBooked >= zoneCapacity)
            {
                return false; // Nema dovoljno mesta
            }
        }
        return true; // Dovoljno mesta za sve dane i zone
    }


    public static string GenerateToken()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 5) 
                                      .Select(s => s[random.Next(s.Length)]).ToArray());
    }


    public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
    {
        return await _context.Bookings
                             .Include(b => b.Customer)
                             .Include(b => b.BookingDayZones)
                                 .ThenInclude(bdz => bdz.RaceDay)
                             .Include(b => b.BookingDayZones)
                                 .ThenInclude(bdz => bdz.SeatingZone)
                             .ToListAsync();
    }
    public async Task<Booking> CreateBookingAsync(Booking booking)
    {

        // Check if booking contains at least one day and each day has a valid zone
        if (booking.BookingDayZones == null || !booking.BookingDayZones.Any() ||
            booking.BookingDayZones.Any(bd => bd.RaceDayId < 1 || bd.RaceDayId > 3 ||
                                               bd.SeatingZoneId < 1 || bd.SeatingZoneId > 4))
        {
            throw new ArgumentException("Each booking must include at least one valid day with a valid zone.");
        }

        if (!await CheckAvailability(booking))
        {
            return null; // Vraća null ako nema dostupnih mesta
        }

       
        booking.Token = GenerateToken();

        
        var (updatedBooking, isPromoCodeValid) = await CalculatePriceAndDiscountAsync(booking);
        booking = updatedBooking;

      
        booking.PromoCodeUsed = isPromoCodeValid;

       
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        // Ažuriranje statusa postojećeg promo koda ako je iskorišćen
        if (isPromoCodeValid && !string.IsNullOrEmpty(booking.GeneratedPromoCode))
        {
            var promo = await _context.PromoCodes
                .FirstOrDefaultAsync(p => p.Code == booking.GeneratedPromoCode && !p.IsUsed);
            if (promo != null)
            {
                promo.UsedByBookingId = booking.Id;
                promo.IsUsed = true;
                _context.PromoCodes.Update(promo);
                await _context.SaveChangesAsync();  
            }
        }

        // Generisanje novog promo koda
        var newPromoCode = new PromoCode
        {
            Code = GeneratePromoCode(),
            BookingId = booking.Id,
            IsUsed = false
        };
        _context.PromoCodes.Add(newPromoCode);
        await _context.SaveChangesAsync();

        // Vraćanje novog promo koda u polju GeneratedPromoCode za dalju upotrebu
        booking.GeneratedPromoCode = newPromoCode.Code;

        return booking;
    }





    private string GeneratePromoCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 6)  // Generiše kod dužine 6 karaktera
                                      .Select(s => s[random.Next(s.Length)]).ToArray());
    }



    public async Task<Booking> GetBookingByIdAsync(int id)
    {
        return await _context.Bookings
                             .Include(b => b.Customer)
                             .Include(b => b.BookingDayZones)
                                 .ThenInclude(bdz => bdz.RaceDay)
                             .Include(b => b.BookingDayZones)
                                 .ThenInclude(bdz => bdz.SeatingZone)
                             .FirstOrDefaultAsync(b => b.Id == id);
    }

    //public async Task<bool> UpdateBookingAsync(Booking booking)
    //{
    //    var existingBooking = await _context.Bookings
    //                                        .Include(b => b.BookingDayZones)
    //                                        .FirstOrDefaultAsync(b => b.Id == booking.Id);

    //    if (existingBooking != null)
    //    {
    //        // Proverava dostupnost pre ažuriranja
    //        if (!await CheckAvailability(booking))
    //        {
    //            return false; // Vraća false ako nema dostupnih mesta
    //        }

    //        // Ažuriranje osnovnih podataka rezervacije
    //        _context.Entry(existingBooking).CurrentValues.SetValues(booking);
    //        existingBooking.BookingDayZones.Clear();

    //        // Ažuriranje detalja o danima i zonama
    //        foreach (var bdz in booking.BookingDayZones)
    //        {
    //            existingBooking.BookingDayZones.Add(new BookingDayZone
    //            {
    //                BookingId = booking.Id,
    //                RaceDayId = bdz.RaceDayId,
    //                SeatingZoneId = bdz.SeatingZoneId
    //            });
    //        }

    //        // Preračunavanje cena i popusta
    //        var (updatedBooking, isPromoCodeValid) = await CalculatePriceAndDiscountAsync(booking);
    //        booking = updatedBooking;
    //        existingBooking.PromoCodeUsed = isPromoCodeValid;
    //        existingBooking.OriginalPrice = booking.OriginalPrice;
    //        existingBooking.FinalPrice = booking.FinalPrice;

    //        // Čuvanje promena u bazi
    //        await _context.SaveChangesAsync();
    //        return true; // Uspešno ažurirana rezervacija
    //    }
    //    return false; // Rezervacija nije pronađena
    //}




    public async Task<bool> DeleteBookingAsync(int id)
    {
        var booking = await _context.Bookings
                                    .Include(b => b.BookingDayZones)
                                    .FirstOrDefaultAsync(b => b.Id == id);

        if (booking != null)
        {
            // Prvo brišemo sve povezane BookingDayZone entitete
            _context.BookingDayZones.RemoveRange(booking.BookingDayZones);

            // Nakon toga možemo sigurno obrisati i sam Booking
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return true;  // Uspešno obrisano
        }
        return false;  // Booking nije pronađen, brisanje nije uspešno
    }




    public async Task<(Booking, bool)> CalculatePriceAndDiscountAsync(Booking booking)
    {
        // Izračunaj osnovnu cenu na osnovu odabranih zona
        var originalPrice = booking.BookingDayZones
            .Select(bdz => _context.SeatingZones.Find(bdz.SeatingZoneId).Price)
            .Sum();

        // Izračunaj popuste na osnovu broja jedinstvenih dana
        var daysBooked = booking.BookingDayZones.Select(bdz => bdz.RaceDayId).Distinct().Count();
        decimal discountRate = daysBooked > 1 ? 0.10m * (daysBooked - 1) : 0m;

        bool isPromoCodeValid = false;

        // Provera da li je promo kod validan preko baze
        if (!string.IsNullOrEmpty(booking.GeneratedPromoCode))
        {

            var promo = await _context.PromoCodes
                .FirstOrDefaultAsync(p => p.Code == booking.GeneratedPromoCode && !p.IsUsed);
            if (promo != null)
            {
               
               

                discountRate += 0.05m;  // Dodaj 5% popusta za validan promo kod
                isPromoCodeValid = true;
            }
        }

        // Provera da li je PromoCodeUsed true za trenutni booking
        var usedPromo = await _context.PromoCodes
         .AnyAsync(p => p.UsedByBookingId == booking.Id);
        if (usedPromo)
        {
            discountRate += 0.05m;  // Dodaj 5% popusta ako je već iskorišćen promo kod za ovaj booking
            isPromoCodeValid = true; 
        }


        // Primena dodatnog popusta za rane prijave
        if (DateTime.Now < earlyBirdCutoff)
        {
            discountRate += 0.10m;
            booking.IsEarlyBird = true;
        }

        booking.OriginalPrice = originalPrice;
        booking.FinalPrice = originalPrice * (1 - discountRate);

        return (booking, isPromoCodeValid);
    }



    public async Task<Booking> GetBookingByTokenAsync(string token, string email)
    {
        return await _context.Bookings
            .Include(b => b.Customer)
            .Include(b => b.BookingDayZones)
                .ThenInclude(bdz => bdz.RaceDay)
            .Include(b => b.BookingDayZones)
                .ThenInclude(bdz => bdz.SeatingZone)
            .Select(b => new Booking
            {
                Id = b.Id,
                CustomerId = b.CustomerId,
                Customer = b.Customer,
                Token = b.Token,
                PromoCodeUsed = b.PromoCodeUsed,
                BookingStatus = b.BookingStatus,
                OriginalPrice = b.OriginalPrice,
                FinalPrice = b.FinalPrice,
                IsEarlyBird = b.IsEarlyBird,
                BookingDayZones = b.BookingDayZones,
                GeneratedPromoCode = _context.PromoCodes
                    .Where(pc => pc.BookingId == b.Id && pc.IsUsed)
                    .Select(pc => pc.Code)
                    .FirstOrDefault()
            })
            .FirstOrDefaultAsync(b => b.Token == token && b.Customer.Email == email);
    }




    public async Task<bool> DeleteBookingByTokenAsync(string token, string email)
    {
        var booking = await _context.Bookings
                                    .Include(b => b.Customer)
                                    .Include(b => b.BookingDayZones)
                                    .FirstOrDefaultAsync(b => b.Token == token && b.Customer.Email == email);

        if (booking != null)
        {
            _context.BookingDayZones.RemoveRange(booking.BookingDayZones);

            var promos = await _context.PromoCodes
                                       .Where(p => p.BookingId == booking.Id)
                                       .ToListAsync();
            foreach (var promo in promos)
            {
                promo.IsUsed = true;
                _context.PromoCodes.Update(promo);
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }



    public async Task<bool> UpdateBookingAsync(Booking updatedBooking, string token, string email)
    {
        var existingBooking = await _context.Bookings
                                            .Include(b => b.Customer)
                                            .Include(b => b.BookingDayZones)
                                            .ThenInclude(bdz => bdz.RaceDay)
                                            .Include(b => b.BookingDayZones)
                                            .ThenInclude(bdz => bdz.SeatingZone)
                                            .FirstOrDefaultAsync(b => b.Token == token && b.Customer.Email == email);

        if (existingBooking != null)
        {
            // Proverava dostupnost pre ažuriranja
            if (!await CheckAvailability(updatedBooking))
            {
                return false; // Vraća false ako nema dostupnih mesta
            }

            // Ažuriranje detalja rezervacije
            existingBooking.BookingStatus = updatedBooking.BookingStatus;
            existingBooking.PromoCodeUsed = updatedBooking.PromoCodeUsed; // prekopiraj status promo koda
            existingBooking.BookingDayZones.Clear();

            // Ažuriranje dana i zona
            foreach (var bdz in updatedBooking.BookingDayZones)
            {
                existingBooking.BookingDayZones.Add(new BookingDayZone
                {
                    BookingId = existingBooking.Id, 
                    RaceDayId = bdz.RaceDayId,
                    SeatingZoneId = bdz.SeatingZoneId
                });
            }

            // Preračunavanje cena i popusta na osnovu ažuriranih podataka
            var (updatedPrice, isPromoCodeValid) = await CalculatePriceAndDiscountAsync(existingBooking);
            existingBooking.OriginalPrice = updatedPrice.OriginalPrice;
            existingBooking.FinalPrice = updatedPrice.FinalPrice;
            existingBooking.PromoCodeUsed = isPromoCodeValid;

            // Čuvanje promena u bazi
            await _context.SaveChangesAsync();
            return true; // Uspešno ažurirana rezervacija
        }
        return false; // Rezervacija nije pronađena
    }













}
