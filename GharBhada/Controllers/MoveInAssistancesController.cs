using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using GharBhada.Repositories.GenericRepositories;
using GharBhada.Models;
using GharBhada.DTOs.MoveInAssistanceDTOs;

namespace GharBhada.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MoveInAssistancesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepositories _genericRepositories;

        public MoveInAssistancesController(IMapper mapper, IGenericRepositories genericRepositories)
        {
            _mapper = mapper;
            _genericRepositories = genericRepositories;
        }

        // GET: api/MoveInAssistances
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MoveInAssistanceReadDTO>>> GetMoveInAssistances()
        {
            var moveInAssistances = await _genericRepositories.SelectAll<MoveInAssistance>();
            return Ok(_mapper.Map<IEnumerable<MoveInAssistanceReadDTO>>(moveInAssistances));
        }

        // GET: api/MoveInAssistances/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MoveInAssistanceReadDTO>> GetMoveInAssistance(int id)
        {
            var moveInAssistance = await _genericRepositories.SelectbyId<MoveInAssistance>(id);
            if (moveInAssistance == null)
            {
                return NotFound(new { message = "MoveInAssistance not found." });
            }
            return Ok(_mapper.Map<MoveInAssistanceReadDTO>(moveInAssistance));
        }

        // PUT: api/MoveInAssistances/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMoveInAssistance(int id, MoveInAssistanceUpdateDTO moveInAssistanceUpdateDTO)
        {
            if (id != moveInAssistanceUpdateDTO.MoveInAssistanceId)
            {
                return BadRequest(new { message = "Mismatched MoveInAssistance ID." });
            }

            var existingMoveInAssistance = await _genericRepositories.SelectbyId<MoveInAssistance>(id);
            if (existingMoveInAssistance == null)
            {
                return NotFound(new { message = "MoveInAssistance not found." });
            }

            _mapper.Map(moveInAssistanceUpdateDTO, existingMoveInAssistance);
            await _genericRepositories.UpdatebyId(id, existingMoveInAssistance);

            return NoContent();
        }

        // POST: api/MoveInAssistances
        [HttpPost]
        public async Task<ActionResult<MoveInAssistanceReadDTO>> PostMoveInAssistance(MoveInAssistanceCreateDTO moveInAssistanceCreateDTO)
        {
            var moveInAssistance = _mapper.Map<MoveInAssistance>(moveInAssistanceCreateDTO);
            await _genericRepositories.Create(moveInAssistance);
            return CreatedAtAction(nameof(GetMoveInAssistance), new { id = moveInAssistance.MoveInAssistanceId }, _mapper.Map<MoveInAssistanceReadDTO>(moveInAssistance));
        }

        // DELETE: api/MoveInAssistances/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMoveInAssistance(int id)
        {
            var moveInAssistance = await _genericRepositories.SelectbyId<MoveInAssistance>(id);
            if (moveInAssistance == null)
            {
                return NotFound(new { message = "MoveInAssistance not found." });
            }

            await _genericRepositories.DeleteById<MoveInAssistance>(id);

            return NoContent();
        }
    }
}