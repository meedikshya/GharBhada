using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GharBhada.Utils
{
    public enum PaymentMethod
    {
        eSewa,
        Khalti
    }

    public enum PaymentVersion
    {
        v1,
        v2
    }

    public enum PaymentMode
    {
        Sandbox,
        Live
    }

    public class PaymentManager
    {
        private readonly HttpClient _httpClient;
        private readonly PaymentMethod _paymentMethod;
        private readonly PaymentVersion _paymentVersion;
        private readonly PaymentMode _paymentMode;
        private readonly string _apiKey;

        // eSewa endpoints
        private readonly string _eSewaLiveEndpoint = "https://esewa.com.np/epay/main";
        private readonly string _eSewaSandboxEndpoint = "https://uat.esewa.com.np/epay/main";

        // Khalti endpoints
        private readonly string _khaltiLiveEndpoint = "https://khalti.com/api/v2";
        private readonly string _khaltiSandboxEndpoint = "https://a.khalti.com/api/v2";

        public PaymentManager(PaymentMethod paymentMethod, PaymentVersion paymentVersion, PaymentMode paymentMode, string apiKey)
        {
            _httpClient = new HttpClient();
            _paymentMethod = paymentMethod;
            _paymentVersion = paymentVersion;
            _paymentMode = paymentMode;
            _apiKey = apiKey;
        }

        public Task<T> InitiatePaymentAsync<T>(dynamic request) where T : class, new()
        {
            if (_paymentMethod == PaymentMethod.eSewa)
            {
                return InitiateEsewaPaymentAsync<T>(request);
            }
            else if (_paymentMethod == PaymentMethod.Khalti)
            {
                return InitiateKhaltiPaymentAsync<T>(request);
            }

            throw new NotImplementedException($"Payment method {_paymentMethod} is not implemented");
        }

        public Task<T> VerifyPaymentAsync<T>(string verificationData) where T : class, new()
        {
            if (_paymentMethod == PaymentMethod.eSewa)
            {
                return VerifyEsewaPaymentAsync<T>(verificationData);
            }
            else if (_paymentMethod == PaymentMethod.Khalti)
            {
                return VerifyKhaltiPaymentAsync<T>(verificationData);
            }

            throw new NotImplementedException($"Payment method {_paymentMethod} is not implemented");
        }

        // Fixed: Removed async keyword since there's no await
        private Task<T> InitiateEsewaPaymentAsync<T>(dynamic request) where T : class, new()
        {
            string endpoint = _paymentMode == PaymentMode.Live ? _eSewaLiveEndpoint : _eSewaSandboxEndpoint;

            // For eSewa, we build a URL with query parameters
            var urlParams = new StringBuilder();
            urlParams.Append($"?amt={request.Amount}");
            urlParams.Append($"&txAmt={request.TaxAmount}");
            urlParams.Append($"&tAmt={request.TotalAmount}");
            urlParams.Append($"&pid={request.TransactionUuid}");
            urlParams.Append($"&scd={request.ProductCode}");
            urlParams.Append($"&psc={request.ProductServiceCharge}");
            urlParams.Append($"&pdc={request.ProductDeliveryCharge}");
            urlParams.Append($"&su={Uri.EscapeDataString(request.SuccessUrl)}");
            urlParams.Append($"&fu={Uri.EscapeDataString(request.FailureUrl)}");

            if (_paymentVersion == PaymentVersion.v2)
            {
                // Add signature for v2 if needed
                string signature = ComputeSignature($"{request.TotalAmount},{request.TransactionUuid},{request.ProductCode}", _apiKey);
                urlParams.Append($"&signature={signature}");
            }

            string redirectUrl = endpoint + urlParams.ToString();

            var response = new ApiResponse
            {
                success = true,
                message = "Payment URL generated successfully",
                data = redirectUrl
            };

            // Return as a completed task instead of using async/await
            return Task.FromResult(JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(response))!);
        }

        private async Task<T> InitiateKhaltiPaymentAsync<T>(dynamic request) where T : class, new()
        {
            string endpoint = _paymentMode == PaymentMode.Live
                ? $"{_khaltiLiveEndpoint}/epayment/initiate/"
                : $"{_khaltiSandboxEndpoint}/epayment/initiate/";

            string jsonContent = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Reset headers to avoid previous request's headers
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Key", _apiKey);

            HttpResponseMessage response = await _httpClient.PostAsync(endpoint, content);
            string responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(responseContent) ?? new T();
            }

            throw new Exception($"Khalti payment initiation failed: {responseContent}");
        }

        // Fixed: Removed async keyword since there's no await
        private Task<T> VerifyEsewaPaymentAsync<T>(string queryParams) where T : class, new()
        {
            // Parse the query parameters
            var parameters = ParseQueryString(queryParams);

            // Create the response object
            var verificationResponse = new eSewaResponse
            {
                status = parameters.GetValueOrDefault("status", ""),
                transaction_code = parameters.GetValueOrDefault("refId", ""),
                total_amount = parameters.GetValueOrDefault("amt", ""),
                transaction_uuid = parameters.GetValueOrDefault("oid", "")
            };

            // Return as a completed task instead of using async/await
            return Task.FromResult(JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(verificationResponse))!);
        }

        private async Task<T> VerifyKhaltiPaymentAsync<T>(string pidx) where T : class, new()
        {
            string endpoint = _paymentMode == PaymentMode.Live
                ? $"{_khaltiLiveEndpoint}/epayment/lookup/?pidx={pidx}"
                : $"{_khaltiSandboxEndpoint}/epayment/lookup/?pidx={pidx}";

            // Reset headers to avoid previous request's headers
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Key", _apiKey);

            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
            string responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(responseContent) ?? new T();
            }

            throw new Exception($"Khalti payment verification failed: {responseContent}");
        }

        private string ComputeSignature(string data, string key)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private Dictionary<string, string> ParseQueryString(string queryString)
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            // If the query string starts with a '?', remove it
            if (queryString.StartsWith("?"))
                queryString = queryString.Substring(1);

            foreach (string param in queryString.Split('&'))
            {
                if (string.IsNullOrEmpty(param))
                    continue;

                string[] parts = param.Split('=');
                string key = parts[0];
                string value = parts.Length > 1 ? Uri.UnescapeDataString(parts[1]) : string.Empty;

                if (!result.ContainsKey(key))
                {
                    result.Add(key, value);
                }
            }

            return result;
        }
    }

    // Response models - no changes needed here
    public class ApiResponse
    {
        public bool success { get; set; }
        public string? message { get; set; }
        public dynamic? data { get; set; }
    }

    public class eSewaResponse
    {
        public string? status { get; set; }
        public string? transaction_code { get; set; }
        public string? total_amount { get; set; }
        public string? transaction_uuid { get; set; }
    }

    public class KhaltiInitResponse
    {
        public string? payment_url { get; set; }
        public string? pidx { get; set; }
    }

    public class KhaltiResponse
    {
        public string? status { get; set; }
        public string? pidx { get; set; }
        public string? total_amount { get; set; }
    }

    public class KhaltiCustomerInfo
    {
        public string? name { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
    }

    public class KhaltiProductDetail
    {
        public string? identity { get; set; }
        public string? name { get; set; }
        public decimal total_price { get; set; }
        public int quantity { get; set; }
        public decimal unit_price { get; set; }
    }

    public class KhaltiAmountBreakdown
    {
        public string? label { get; set; }
        public decimal amount { get; set; }
    }
}