using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using AutoMapper;
using inventoryAppDomain.Entities;
using inventoryAppDomain.Entities.Dtos;
using inventoryAppDomain.Entities.MonnifyDtos;
using inventoryAppDomain.IdentityEntities;
using inventoryAppDomain.Services;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;

namespace inventoryAppDomain.Repository
{
    public class MonnifyPaymentService : IPaymentService
    {
        private readonly IOrderService _orderService;
        private readonly ITransactionService _transactionService;
        private readonly ApplicationDbContext _dbContext;
        private string _contractCode = WebConfigurationManager.AppSettings["monnifyContractCode"];
        private string _loginUrl = "https://sandbox.monnify.com/api/v1/auth/login";
        private string _initTransactionUrl = "https://sandbox.monnify.com/api/v1/merchant/transactions/init-transaction";
        private string _verifyTransactionUrl = "https://sandbox.monnify.com/api/v2/transactions/";
        private string _secretKey = WebConfigurationManager.AppSettings["monnifySecretKey"];
        private string _apiKey = WebConfigurationManager.AppSettings["monnifyApiKey"];

        public MonnifyPaymentService(IOrderService orderService, ITransactionService transactionService)
        {
            _orderService = orderService;
            _transactionService = transactionService;
            _dbContext = HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();
        }
        
        private string GenerateTxRef()
        {
            const int txRefLength = 11;
        
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToLower();
            var random = new Random();
        
            var txRef = "MC-" + new string(Enumerable.Repeat(chars, txRefLength)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        
            var txRefInDb = _dbContext.Transactions.SingleOrDefault(t => t.GeneratedReferenceNumber == txRef);
        
            if (txRefInDb != null)
                GenerateTxRef();
        
            return txRef;
        }

        private async Task<ResponseDto> Login()
        {
            var httpClient = new HttpClient {BaseAddress = new Uri(_loginUrl)};
            var encodedString = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_apiKey}:{_secretKey}"));
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {encodedString}");

            var response = await httpClient.PostAsync(_loginUrl, null);
            return JsonConvert.DeserializeObject<ResponseDto>(await response.Content.ReadAsStringAsync());
        }
        public async Task<InitTransactionResponseBody> InitiatePayment(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            var payload = Mapper.Map<Order, TransactionPayload>(order);
            payload.contractCode = _contractCode;
            payload.paymentReference = GenerateTxRef();
            payload.paymentDescription = $"Order Payment for {payload.customerName}";
            var httpClient = new HttpClient();
            var encodedString = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_apiKey}:{_secretKey}"));
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {encodedString}");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(_initTransactionUrl, content);

            var responseContent = JsonConvert.DeserializeObject<ResponseDto>(await response.Content.ReadAsStringAsync());
            var responseBody =
                JsonConvert.DeserializeObject<InitTransactionResponseBody>(responseContent.responseBody.ToString());
            await _transactionService.CreateTransaction(responseBody.transactionReference, payload.paymentReference, orderId);
            return responseBody;
        }

        public async Task<bool> VerifyPayment(string paymentReference)
        {
            var accessToken = JsonConvert.DeserializeObject<AccessTokenResponseBody>(JsonConvert.SerializeObject((await Login()).responseBody)).accessToken;
            var transaction = await _transactionService.GetTransactionByGeneratedRef(paymentReference);
            var encodedUrl = HttpUtility.UrlEncode(transaction.ReferenceNumber);
            var fullPath = $"{_verifyTransactionUrl}{encodedUrl}";
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.GetAsync(fullPath);
            var responseContent =
                JsonConvert.DeserializeObject<ResponseDto>(await response.Content.ReadAsStringAsync());

            var responseBody =
                JsonConvert.DeserializeObject<VerificationResponseBody>(responseContent.responseBody.ToString());

            if (responseBody.paymentStatus.Equals("PAID"))
            {
                return true;
            }

            return false;
        }

    }
}