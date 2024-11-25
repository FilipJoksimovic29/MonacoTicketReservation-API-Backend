using Microsoft.AspNetCore.Mvc;
using RezervacijaKarata.Services.Interfaces;
using RezervacijaKarataAPI.DTO;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBookings()
    {
        var bookings = await _bookingService.GetAllBookingsAsync();
        var bookingsDto = bookings.Select(BookingDTOMapper.ToDTO).ToList();
        return Ok(bookingsDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookingById(int id)
    {
        var booking = await _bookingService.GetBookingByIdAsync(id);
        if (booking == null)
            return NotFound();
        return Ok(BookingDTOMapper.ToDTO(booking));
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] BookingDTO bookingDto)
    {
        var booking = BookingDTOMapper.ToBooking(bookingDto);
        var createdBooking = await _bookingService.CreateBookingAsync(booking);
        return CreatedAtAction(nameof(GetBookingById), new { id = createdBooking.Id }, BookingDTOMapper.ToDTO(createdBooking));
    }

    //[HttpPut("{id}")]
    //public async Task<IActionResult> UpdateBooking(int id, [FromBody] BookingDTO bookingDto)
    //{
    //    var booking = BookingDTOMapper.ToBooking(bookingDto);
    //    booking.Id = id;  // Ensure correct ID is set
    //    await _bookingService.UpdateBookingAsync(booking);
    //    return NoContent();
    //}

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBooking(int id)
    {
        await _bookingService.DeleteBookingAsync(id);
        return NoContent();
    }

    [HttpPost("CalculatePriceAndDiscount")]
    public async Task<IActionResult> CalculatePriceAndDiscountAsync([FromBody] BookingDTO bookingDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var booking = BookingDTOMapper.ToBooking(bookingDto);
        var (updatedBooking, _) = await _bookingService.CalculatePriceAndDiscountAsync(booking);

        return Ok(BookingDTOMapper.ToDTO(updatedBooking));
    }

    [HttpGet("get/{token}/{email}")]
    public async Task<IActionResult> GetBookingByToken(string token, string email)
    {
        var booking = await _bookingService.GetBookingByTokenAsync(token, email);
        if (booking == null)
        {
            return NotFound(new { Message = "Booking with the provided token and email does not exist." });
        }
        return Ok(BookingDTOMapper.ToDTO(booking));
    }

    [HttpDelete("delete/by-token/{token}/{email}")]
    public async Task<IActionResult> DeleteBookingByToken(string token, string email)
    {
        bool deleted = await _bookingService.DeleteBookingByTokenAsync(token, email);
        if (!deleted)
        {
            return NotFound(new { Message = "No booking found with the provided token and email, or deletion failed." });
        }
        return NoContent(); 
    }



    [HttpPut("update/{token}/{email}")]
    public async Task<IActionResult> UpdateBooking([FromBody] BookingDTO bookingDto, string token, string email)
    {
        var booking = BookingDTOMapper.ToBooking(bookingDto);
        bool updated = await _bookingService.UpdateBookingAsync(booking, token, email);
        if (!updated)
        {
            return NotFound(new { Message = "No booking found with the provided token and email, or update failed." });
        }
        return NoContent(); 
    }



}
