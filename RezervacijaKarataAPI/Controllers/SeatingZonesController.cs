using Microsoft.AspNetCore.Mvc;
using RezervacijaKarataAPI.DTO;
using RezervacijaKarata.Services.Interfaces;
using System.Threading.Tasks;

namespace RezervacijaKarataAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeatingZonesController : ControllerBase
    {
        private readonly ISeatingZoneService _seatingZoneService;

        public SeatingZonesController(ISeatingZoneService seatingZoneService)
        {
            _seatingZoneService = seatingZoneService;
        }

       // [HttpGet]
        //public async Task<IActionResult> GetAllSeatingZones()
        //{
        //    var seatingZones = await _seatingZoneService.GetAllSeatingZonesAsync();
        //    var seatingZoneDtos = seatingZones.Select(SeatingZoneDTOMapper.ToDTO).ToList();
        //    return Ok(seatingZoneDtos);
        //}

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSeatingZone(int id)
        {
            var seatingZone = await _seatingZoneService.GetSeatingZoneByIdAsync(id);
            if (seatingZone == null)
            {
                return NotFound();
            }
            return Ok(SeatingZoneDTOMapper.ToDTO(seatingZone));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSeatingZones([FromQuery] int? capacity, [FromQuery] decimal? price, [FromQuery] bool? isAccessible, [FromQuery] bool? hasLargeTV, [FromQuery] string name)
        {
            var seatingZones = await _seatingZoneService.GetAllSeatingZonesAsync(capacity, price, isAccessible, hasLargeTV, name);
            var seatingZoneDtos = seatingZones.Select(SeatingZoneDTOMapper.ToDTO).ToList();
            return Ok(seatingZoneDtos);
        }


        [HttpPost]
        public async Task<IActionResult> CreateSeatingZone([FromBody] SeatingZoneDTO seatingZoneDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var seatingZone = SeatingZoneDTOMapper.ToSeatingZone(seatingZoneDto);
            var createdSeatingZone = await _seatingZoneService.CreateSeatingZoneAsync(seatingZone);
            return CreatedAtAction(nameof(GetSeatingZone), new { id = createdSeatingZone.Id }, SeatingZoneDTOMapper.ToDTO(createdSeatingZone));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeatingZone(int id, [FromBody] SeatingZoneDTO seatingZoneDto)
        {
            if (id != seatingZoneDto.Id)
            {
                return BadRequest("Mismatched ID");
            }

            var seatingZone = SeatingZoneDTOMapper.ToSeatingZone(seatingZoneDto);
            await _seatingZoneService.UpdateSeatingZoneAsync(seatingZone);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeatingZone(int id)
        {
            await _seatingZoneService.DeleteSeatingZoneAsync(id);
            return NoContent();
        }
    }
}
