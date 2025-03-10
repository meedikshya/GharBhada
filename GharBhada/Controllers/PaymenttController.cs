using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GharBhada.Utils;
using GharBhada.Models;
using GharBhada.Repositories.GenericRepositories;
using System.Collections.Generic;

namespace GharBhada.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IGenericRepositories _genericRepositories;
        private readonly string eSewa_SecretKey = "8gBm/:&EnhH.1/q";
        private readonly string Khalti_SecretKey = "live_secret_key_68791341fdd94846a146f0457ff7b455";
        private readonly bool sandBoxMode = true;

        // Constructor to inject the repositories
        public PaymentController(IGenericRepositories genericRepositories)
        {
            _genericRepositories = genericRepositories;
        }

        // POST: api/Payment/pay-with-esewa
        [HttpPost("pay-with-esewa")]
        public async Task<IActionResult> PayWithEsewa([FromBody] PaymentRequest request = null)
        {
            try
            {
                var paymentManager = new PaymentManager(
                    PaymentMethod.eSewa,
                    PaymentVersion.v2,
                    sandBoxMode ? PaymentMode.Sandbox : PaymentMode.Live,
                    eSewa_SecretKey
                );

                // Default values if request is null (for testing)
                decimal amount = request?.Amount ?? 100;
                decimal taxAmount = request?.TaxAmount ?? 10;
                decimal totalAmount = request?.TotalAmount ?? 110;
                int agreementId = request?.AgreementId ?? 1;
                int renterId = request?.RenterId ?? 1;

                // Generate a unique transaction ID
                string transactionId = "tx-" + Guid.NewGuid().ToString("N").Substring(0, 8);

                // Get full base URL
                string currentUrl = $"{Request.Scheme}://{Request.Host}";

                // Create request object
                var paymentRequest = new
                {
                    Amount = amount,
                    TaxAmount = taxAmount,
                    TotalAmount = totalAmount,
                    TransactionUuid = transactionId,
                    ProductCode = sandBoxMode ? "EPAYTEST" : "YOUR_PRODUCT_CODE",
                    ProductServiceCharge = 0,
                    ProductDeliveryCharge = 0,
                    SuccessUrl = $"{currentUrl}/api/Payment/success",
                    FailureUrl = $"{currentUrl}/api/Payment/failure",
                    SignedFieldNames = "total_amount,transaction_uuid,product_code"
                };

                // Initialize payment
                var response = await paymentManager.InitiatePaymentAsync<ApiResponse>(paymentRequest);

                // Create payment record in database if needed
                if (_genericRepositories != null && request != null)
                {
                    var paymentRecord = new Payment
                    {
                        AgreementId = agreementId,
                        RenterId = renterId,
                        Amount = totalAmount,
                        PaymentStatus = "Pending",
                        TransactionId = transactionId,
                        PaymentGateway = "eSewa"
                    };

                    await _genericRepositories.Create(paymentRecord);
                }

                // Return redirect URL to client
                return Ok(new { redirectUrl = response.data.ToString() });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Payment initiation failed: {ex.Message}" });
            }
        }

        // GET: api/Payment/success
        [HttpGet("success")]
        public async Task<IActionResult> Success(
            [FromQuery] string oid,
            [FromQuery] string amt,
            [FromQuery] string refId,
            [FromQuery] string status)
        {
            try
            {
                // For testing and debugging
                Console.WriteLine($"Success callback received: oid={oid}, amt={amt}, refId={refId}, status={status}");

                // Check if payment was successful
                bool isSuccess = !string.IsNullOrEmpty(status) &&
                               status.Equals("success", StringComparison.OrdinalIgnoreCase);

                // If payment is successful and we can access the database
                if (isSuccess && _genericRepositories != null && !string.IsNullOrEmpty(oid))
                {
                    try
                    {
                        // Find the payment by transaction ID
                        var payments = await _genericRepositories.SelectAll<Payment>(p => p.TransactionId == oid);
                        var payment = payments.FirstOrDefault();

                        if (payment != null)
                        {
                            // Update payment status
                            payment.PaymentStatus = "Completed";
                            payment.ReferenceId = refId;
                            await _genericRepositories.UpdatebyId(payment.PaymentId, payment);
                        }
                    }
                    catch (Exception dbEx)
                    {
                        Console.WriteLine($"Database error: {dbEx.Message}");
                        // Continue even if database update fails
                    }
                }

                // Return HTML for testing
                string htmlResponse = $@"
                <html>
                    <head>
                        <title>Payment {(isSuccess ? "Successful" : "Failed")}</title>
                        <style>
                            body {{ font-family: Arial, sans-serif; text-align: center; padding: 40px; }}
                            .container {{ max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 8px; }}
                            .success {{ color: green; }}
                            .failure {{ color: red; }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <h1 class='{(isSuccess ? "success" : "failure")}'>{(isSuccess ? "Payment Successful" : "Payment Failed")}</h1>
                            <p>Transaction ID: {oid}</p>
                            <p>Reference ID: {refId}</p>
                            <p>Amount: {amt}</p>
                            <p>Status: {status}</p>
                        </div>
                    </body>
                </html>";

                return Content(htmlResponse, "text/html");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error processing callback: {ex.Message}" });
            }
        }

        // GET: api/Payment/failure
        [HttpGet("failure")]
        public IActionResult Failure([FromQuery] string pid)
        {
            string htmlResponse = $@"
            <html>
                <head>
                    <title>Payment Failed</title>
                    <style>
                        body {{ font-family: Arial, sans-serif; text-align: center; padding: 40px; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 8px; }}
                        .failure {{ color: red; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h1 class='failure'>Payment Failed</h1>
                        <p>Your payment was canceled or failed to process.</p>
                        <p>Transaction ID: {pid}</p>
                    </div>
                </body>
            </html>";

            return Content(htmlResponse, "text/html");
        }

        // POST: api/Payment/verify-esewa-payment
        [HttpPost("verify-esewa-payment")]
        public async Task<IActionResult> VerifyEsewaPayment([FromBody] string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return BadRequest(new { message = "Invalid verification data" });
            }

            try
            {
                var paymentManager = new PaymentManager(
                    PaymentMethod.eSewa,
                    PaymentVersion.v2,
                    sandBoxMode ? PaymentMode.Sandbox : PaymentMode.Live,
                    eSewa_SecretKey
                );

                var response = await paymentManager.VerifyPaymentAsync<eSewaResponse>(data);

                if (response != null && string.Equals(response.status, "complete", StringComparison.OrdinalIgnoreCase))
                {
                    return Ok(new
                    {
                        success = true,
                        message = "Payment verified successfully",
                        transaction_code = response.transaction_code,
                        amount = response.total_amount
                    });
                }

                return BadRequest(new { success = false, message = "Payment verification failed" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Verification failed: {ex.Message}" });
            }
        }

        // GET: api/Payment/test
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new { message = "Payment API is working correctly", timestamp = DateTime.Now });
        }
    }

    // Request model class
    public class PaymentRequest
    {
        public int AgreementId { get; set; }
        public int RenterId { get; set; }
        public decimal Amount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Description { get; set; }
    }
}