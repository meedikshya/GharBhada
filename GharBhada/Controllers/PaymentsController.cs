using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using GharBhada.Repositories.GenericRepositories;
using GharBhada.Repositories.SpecificRepositories.PaymentRepositories;
using GharBhada.Models;
using GharBhada.DTOs.PaymentDTOs;

namespace GharBhada.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepositories _genericRepositories;
        private readonly IPaymentRepositories _paymentRepositories;

        public PaymentsController(IMapper mapper, IGenericRepositories genericRepositories, IPaymentRepositories paymentRepositories)
        {
            _mapper = mapper;
            _genericRepositories = genericRepositories;
            _paymentRepositories = paymentRepositories;
        }

        // GET: api/Payments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentReadDTO>>> GetPayments()
        {
            var payments = await _genericRepositories.SelectAll<Payment>();
            return Ok(_mapper.Map<IEnumerable<PaymentReadDTO>>(payments));
        }

        // GET: api/Payments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentReadDTO>> GetPayment(int id)
        {
            var payment = await _genericRepositories.SelectbyId<Payment>(id);
            if (payment == null)
            {
                return NotFound(new { message = "Payment not found." });
            }
            return Ok(_mapper.Map<PaymentReadDTO>(payment));
        }

        // GET: api/Payments/completed-by-landlord/28
        [HttpGet("completed-by-landlord/{landlordId}")]
        public async Task<ActionResult<IEnumerable<PaymentReadDTO>>> GetCompletedPaymentsByLandlordId(int landlordId)
        {
            var payments = await _paymentRepositories.GetCompletedPaymentsByLandlordIdAsync(landlordId);
            return Ok(_mapper.Map<IEnumerable<PaymentReadDTO>>(payments));
        }


        // GET: api/Payments/byAgreementId/5?status=Completed
        [HttpGet("byAgreementId/{agreementId}")]
        public async Task<ActionResult<IEnumerable<PaymentReadDTO>>> GetPaymentsByAgreementId(int agreementId, [FromQuery] string status)
        {
            var payments = await _paymentRepositories.GetPaymentsByAgreementIdAsync(agreementId, status);
            if (payments == null || payments.Count == 0)
            {
                return Ok();
            }
            return Ok(_mapper.Map<IEnumerable<PaymentReadDTO>>(payments));
        }

        // PUT: api/Payments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayment(int id, PaymentUpdateDTO paymentUpdateDTO)
        {
            if (id != paymentUpdateDTO.PaymentId)
            {
                return BadRequest(new { message = "Mismatched payment ID." });
            }

            var existingPayment = await _genericRepositories.SelectbyId<Payment>(id);
            if (existingPayment == null)
            {
                return NotFound(new { message = "Payment not found." });
            }

            _mapper.Map(paymentUpdateDTO, existingPayment);
            await _genericRepositories.UpdatebyId(id, existingPayment);

            return NoContent();
        }

        // POST: api/Payments
        [HttpPost]
        public async Task<ActionResult<PaymentReadDTO>> PostPayment(PaymentCreateDTO paymentCreateDTO)
        {
            var payment = _mapper.Map<Payment>(paymentCreateDTO);
            await _genericRepositories.Create(payment);
            return CreatedAtAction(nameof(GetPayment), new { id = payment.PaymentId }, _mapper.Map<PaymentReadDTO>(payment));
        }

        // DELETE: api/Payments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var payment = await _genericRepositories.SelectbyId<Payment>(id);
            if (payment == null)
            {
                return NotFound(new { message = "Payment not found." });
            }

            await _genericRepositories.DeleteById<Payment>(id);

            return NoContent();
        }
    }
}