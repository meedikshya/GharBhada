using System.Collections.Generic;
using System.Linq;
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
    //[Authorize]
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

        // GET: api/Payments/completed-by-renter/30
        [HttpGet("completed-by-renter/{renterId}")]
        public async Task<ActionResult<IEnumerable<PaymentReadDTO>>> GetCompletedPaymentsByRenterId(int renterId)
        {
            var payments = await _paymentRepositories.GetCompletedPaymentsByRenterIdAsync(renterId);
            return Ok(_mapper.Map<IEnumerable<PaymentReadDTO>>(payments));
        }

        // GET: api/Payments/completed-by-renter-with-property/30
        [HttpGet("completed-by-renter-with-property/{renterId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetCompletedPaymentsWithPropertyByRenterId(int renterId)
        {
            var paymentsWithProperties = await _paymentRepositories.GetCompletedPaymentsWithPropertyByRenterIdAsync(renterId);
            var result = paymentsWithProperties.Select(pwp => new
            {
                Payment = _mapper.Map<PaymentReadDTO>(pwp.Payment),
                Property = pwp.Property
            });
            return Ok(result);
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

        // GET: api/Payments/properties-with-completed-payments
        [HttpGet("properties-with-completed-payments")]
        public async Task<ActionResult<IEnumerable<Property>>> GetPropertiesWithCompletedPayments()
        {
            var properties = await _paymentRepositories.GetPropertiesWithCompletedPaymentsAsync();
            return Ok(properties);
        }

        // GET: api/Payments/completed-payment-count
        [HttpGet("completed-payment-count")]
        public async Task<ActionResult<int>> GetCompletedPaymentCount()
        {
            var count = await _paymentRepositories.GetCompletedPaymentCountAsync();
            return Ok(count);
        }

        // GET: api/Payments/completed-with-details
        [HttpGet("completed-with-details")]
        public async Task<ActionResult<IEnumerable<PaymentReadDTO>>> GetCompletedPaymentsWithDetails()
        {
            var payments = await _paymentRepositories.GetCompletedPaymentsWithDetailsAsync();
            if (payments == null || payments.Count == 0)
            {
                return NotFound(new { message = "No completed payments found." });
            }
            var result = payments.Select(p => new
            {
                Payment = _mapper.Map<PaymentReadDTO>(p.Payment),
                Property = p.Property,
                RenterId = p.RenterId,
                LandlordId = p.LandlordId
            });
            return Ok(result);
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