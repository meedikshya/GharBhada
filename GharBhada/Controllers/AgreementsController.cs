using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using GharBhada.Repositories.GenericRepositories;
using GharBhada.DTOs.AgreementDTOs;
using GharBhada.Models;
using GharBhada.Repositories.SpecificRepositories.AgreementRepositories;
using GharBhada.DTOs;

namespace GharBhada.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AgreementsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepositories _genericRepositories;
        private readonly IAgreementRepositories _agreementRepositories;

        public AgreementsController(IMapper mapper, IGenericRepositories genericRepositories, IAgreementRepositories agreementRepositories)
        {
            _mapper = mapper;
            _genericRepositories = genericRepositories;
            _agreementRepositories = agreementRepositories;
        }

        // GET: api/Agreements
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AgreementReadDTO>>> GetAgreements()
        {
            var agreements = await _genericRepositories.SelectAll<Agreement>();
            return Ok(_mapper.Map<IEnumerable<AgreementReadDTO>>(agreements));
        }

        // GET: api/Agreements/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AgreementReadDTO>> GetAgreement(int id)
        {
            var agreement = await _genericRepositories.SelectbyId<Agreement>(id);
            if (agreement == null)
            {
                return Ok(new { message = "Agreement not found." });
            }
            return Ok(_mapper.Map<AgreementReadDTO>(agreement));
        }

        // GET: api/Agreements/byBookingId/{bookingId}
        [HttpGet("byBookingId/{bookingId}")]
        public async Task<ActionResult<AgreementReadDTO>> GetAgreementByBookingId(int bookingId)
        {
            var agreement = await _agreementRepositories.GetAgreementByBookingIdAsync(bookingId);
            if (agreement == null)
            {
                return Ok();
            }
            return Ok(_mapper.Map<AgreementReadDTO>(agreement));
        }

        // GET: api/Agreements/User/5
        [HttpGet("User/{userId}")]
        public async Task<ActionResult<IEnumerable<AgreementReadDTO>>> GetAgreementsByUserId(int userId)
        {
            var agreements = await _agreementRepositories.GetAgreementsByUserIdAsync(userId);
            if (agreements == null || agreements.Count == 0)
            {
                return NotFound(new { message = "No agreements found for this user." });
            }
            return Ok(_mapper.Map<IEnumerable<AgreementReadDTO>>(agreements));
        }

        // GET: api/Agreements/Landlord/5
        [HttpGet("Landlord/{landlordId}")]
        public async Task<ActionResult<IEnumerable<AgreementReadDTO>>> GetAgreementsByLandlordId(int landlordId)
        {
            var agreements = await _agreementRepositories.GetAgreementsByLandlordIdAsync(landlordId);
            if (agreements == null || agreements.Count == 0)
            {
                return Ok(new { message = "No agreements found for this landlord." });
            }
            return Ok(_mapper.Map<IEnumerable<AgreementReadDTO>>(agreements));
        }


        // GET: api/Agreements/total-count
        [HttpGet("total-count")]
        public async Task<ActionResult<int>> GetTotalAgreementCount()
        {
            var count = await _agreementRepositories.GetTotalAgreementCountAsync();
            return Ok(count);
        }

        // GET: api/Agreements/approved-count
        [HttpGet("approved-count")]
        public async Task<ActionResult<int>> GetApprovedAgreementCount()
        {
            var count = await _agreementRepositories.GetApprovedAgreementCountAsync();
            return Ok(count);
        }

        // GET: api/Agreements/expired
        [HttpGet("expired")]
        public async Task<ActionResult<IEnumerable<AgreementReadDTO>>> GetExpiredAgreements()
        {
            var agreements = await _agreementRepositories.GetExpiredAgreementsAsync();
            if (agreements == null || agreements.Count == 0)
            {
                return Ok(new { message = "No expired agreements found." });
            }
            return Ok(_mapper.Map<IEnumerable<AgreementReadDTO>>(agreements));
        }

        // GET: api/Agreements/expired-by-renter/5
        [HttpGet("expired-by-renter/{renterId}")]
        public async Task<ActionResult<IEnumerable<AgreementReadDTO>>> GetExpiredAgreementsByRenterId(int renterId)
        {
            var agreements = await _agreementRepositories.GetExpiredAgreementsByRenterIdAsync(renterId);
            if (agreements == null || agreements.Count == 0)
            {
                return Ok(new { message = "No expired agreements found for this renter." });
            }
            return Ok(_mapper.Map<IEnumerable<AgreementReadDTO>>(agreements));
        }


        // PUT: api/Agreements/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAgreement(int id, AgreementUpdateDTO agreementUpdateDTO)
        {
            if (id != agreementUpdateDTO.AgreementId)
            {
                return BadRequest(new { message = "Mismatched agreement ID." });
            }

            var existingAgreement = await _genericRepositories.SelectbyId<Agreement>(id);
            if (existingAgreement == null)
            {
                return Ok(new { message = "Agreement not found." });
            }

            _mapper.Map(agreementUpdateDTO, existingAgreement);
            await _genericRepositories.UpdatebyId(id, existingAgreement);

            return NoContent();
        }

        [HttpPost("trigger-expired-check")]
        public async Task<IActionResult> TriggerExpiredCheck()
        {
            try
            {
                await _agreementRepositories.UpdatePropertyStatusForAllExpiredAgreementsAsync();
                return Ok(new { message = "Expired agreements updated, bookings status updated, and property statuses updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        // POST: api/Agreements
        [HttpPost]
        public async Task<ActionResult<AgreementReadDTO>> PostAgreement(AgreementCreateDTO agreementCreateDTO)
        {
            var agreement = _mapper.Map<Agreement>(agreementCreateDTO);
            await _genericRepositories.Create(agreement);

            return CreatedAtAction(nameof(GetAgreement), new { id = agreement.AgreementId }, _mapper.Map<AgreementReadDTO>(agreement));
        }


        // DELETE: api/Agreements/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgreement(int id)
        {
            var agreement = await _genericRepositories.SelectbyId<Agreement>(id);
            if (agreement == null)
            {
                return NotFound(new { message = "Agreement not found." });
            }

            await _genericRepositories.DeleteById<Agreement>(id);

            return NoContent();
        }
    }
}