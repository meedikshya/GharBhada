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
    public class AgreementsController : ControllerBase
    {
        private readonly GharBhadaContext _context;

        public AgreementsController(GharBhadaContext context)
        {
            _context = context;
        }

        // GET: api/Agreements
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Agreement>>> GetAgreements()
        {
            return await _context.Agreements.ToListAsync();
        }

        // GET: api/Agreements/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Agreement>> GetAgreement(int id)
        {
            var agreement = await _context.Agreements.FindAsync(id);

            if (agreement == null)
            {
                return NotFound();
            }

            return agreement;
        }

        // PUT: api/Agreements/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAgreement(int id, Agreement agreement)
        {
            if (id != agreement.AgreementId)
            {
                return BadRequest();
            }

            _context.Entry(agreement).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgreementExists(id))
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

        // POST: api/Agreements
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Agreement>> PostAgreement(Agreement agreement)
        {
            _context.Agreements.Add(agreement);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAgreement", new { id = agreement.AgreementId }, agreement);
        }

        // DELETE: api/Agreements/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgreement(int id)
        {
            var agreement = await _context.Agreements.FindAsync(id);
            if (agreement == null)
            {
                return NotFound();
            }

            _context.Agreements.Remove(agreement);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AgreementExists(int id)
        {
            return _context.Agreements.Any(e => e.AgreementId == id);
        }
    }
}
