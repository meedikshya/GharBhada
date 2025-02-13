using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GharBhada.Data;
using GharBhada.Models;
using Microsoft.AspNetCore.Authorization;

namespace GharBhada.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MoveInAssistancesController : ControllerBase
    {
        private readonly GharBhadaContext _context;

        public MoveInAssistancesController(GharBhadaContext context)
        {
            _context = context;
        }

        // GET: api/MoveInAssistances
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MoveInAssistance>>> GetMoveInAssistances()
        {
            return await _context.MoveInAssistances.ToListAsync();
        }

        // GET: api/MoveInAssistances/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MoveInAssistance>> GetMoveInAssistance(int id)
        {
            var moveInAssistance = await _context.MoveInAssistances.FindAsync(id);

            if (moveInAssistance == null)
            {
                return NotFound();
            }

            return moveInAssistance;
        }

        // PUT: api/MoveInAssistances/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMoveInAssistance(int id, MoveInAssistance moveInAssistance)
        {
            if (id != moveInAssistance.AssistanceId)
            {
                return BadRequest();
            }

            _context.Entry(moveInAssistance).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MoveInAssistanceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/MoveInAssistances
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MoveInAssistance>> PostMoveInAssistance(MoveInAssistance moveInAssistance)
        {
            _context.MoveInAssistances.Add(moveInAssistance);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMoveInAssistance", new { id = moveInAssistance.AssistanceId }, moveInAssistance);
        }

        // DELETE: api/MoveInAssistances/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMoveInAssistance(int id)
        {
            var moveInAssistance = await _context.MoveInAssistances.FindAsync(id);
            if (moveInAssistance == null)
            {
                return NotFound();
            }

            _context.MoveInAssistances.Remove(moveInAssistance);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MoveInAssistanceExists(int id)
        {
            return _context.MoveInAssistances.Any(e => e.AssistanceId == id);
        }
    }
}
