using Microsoft.AspNetCore.Mvc;
using RezervacijaKarataAPI.DTO;
using RezervacijaKarata.Services.Interfaces;
using System.Threading.Tasks;

namespace RezervacijaKarataAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RaceDaysController : ControllerBase
    {
        private readonly IRaceDayService _raceDayService;

        public RaceDaysController(IRaceDayService raceDayService)
        {
            _raceDayService = raceDayService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRaceDays()
        {
            var raceDays = await _raceDayService.GetAllRaceDaysAsync();
            var raceDaysDto = raceDays.Select(RaceDayDTOMapper.ToDTO).ToList();
            return Ok(raceDaysDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRaceDay(int id)
        {
            var raceDay = await _raceDayService.GetRaceDayByIdAsync(id);
            if (raceDay == null)
                return NotFound();
            return Ok(RaceDayDTOMapper.ToDTO(raceDay));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRaceDay([FromBody] RaceDayDTO raceDayDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var raceDay = RaceDayDTOMapper.ToRaceDay(raceDayDto);
            var createdRaceDay = await _raceDayService.CreateRaceDayAsync(raceDay);
            return CreatedAtAction(nameof(GetRaceDay), new { id = createdRaceDay.Id }, RaceDayDTOMapper.ToDTO(createdRaceDay));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRaceDay(int id, [FromBody] RaceDayDTO raceDayDto)
        {
            if (id != raceDayDto.Date.GetHashCode())  
                return BadRequest("Mismatched ID");

            var raceDay = RaceDayDTOMapper.ToRaceDay(raceDayDto);
            raceDay.Id = id; 
            await _raceDayService.UpdateRaceDayAsync(raceDay);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRaceDay(int id)
        {
            await _raceDayService.DeleteRaceDayAsync(id);
            return NoContent();
        }
    }
}
