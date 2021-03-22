using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using AutoMapper;
using inventoryAppDomain.Entities.Dtos;
using inventoryAppDomain.IdentityEntities;
using inventoryAppDomain.Infrastructure;
using inventoryAppDomain.Services;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;

namespace inventoryAppDomain.Repository
{
    // public class FlutterWavePaymentService : IPaymentService
    // {
    //     private readonly IOrderService _orderService;
    //     private readonly ITransactionService _transactionService;
    //     private readonly ApplicationDbContext _dbContext;
    //
    //     public FlutterWavePaymentService(IOrderService orderService, ITransactionService transactionService)
    //     {
    //         _orderService = orderService;
    //         _transactionService = transactionService;
    //         _dbContext = HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();
    //     }
    //
    //     private string GenerateTxRef()
    //     {
    //         const int txRefLength = 5;
    //
    //         var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToLower();
    //         var random = new Random();
    //
    //         var txRef = "MC-" + new string(Enumerable.Repeat(chars, txRefLength)
    //             .Select(s => s[random.Next(s.Length)]).ToArray()) + DateTime.Now;
    //
    //         var txRefInDb = _dbContext.Transactions.SingleOrDefault(t => t.ReferenceNumber == txRef);
    //
    //         if (txRefInDb != null)
    //             GenerateTxRef();
    //
    //         return txRef;
    //     }
    //
    //     public async Task<ChargeResponse> InitiatePayment(TransactionDetails transactionDetails, int orderId, string pin)
    //     {
    //         var raveUrl = "https://ravesandboxapi.flutterwave.com/flwv3-pug/getpaidx/api/charge";
    //         // var raveUrl = "https://api.ravepay.co/flwv3-pug/getpaidx/api/charge";
    //         var publicKey = WebConfigurationManager.AppSettings["flutterWavePublicKey"];
    //         transactionDetails.txRef = GenerateTxRef();
    //         transactionDetails.amount =
    //             Convert.ToString((int)_orderService.GetOrderById(orderId).Price, CultureInfo.InvariantCulture);
    //         transactionDetails.PBFPubKey = publicKey;
    //
    //         try
    //         {
    //             var tdesKey = TdesEncryption.GetEncryptionKey(publicKey);
    //             var encryptionString = TdesEncryption.Encrypt(JsonConvert.SerializeObject(transactionDetails), tdesKey);
    //             var decryptedString = TdesEncryption.Decrypt(encryptionString, tdesKey);
    //             Console.WriteLine(decryptedString);
    //             await _transactionService.CreateTransaction(transactionDetails.txRef, orderId);
    //
    //             var httpClient = new HttpClient();
    //             httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    //
    //             var payload = new InitiatePaymentRequest
    //             {
    //                 PBFPubKey = transactionDetails.PBFPubKey,
    //                 client = encryptionString
    //             };
    //             
    //             var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
    //             
    //             var response = await httpClient.PostAsync(raveUrl, content);
    //             Console.WriteLine(JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync()));
    //             var responseBody = (InitiateSugestionResponse) JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
    //
    //             if (responseBody != null && responseBody.status.Equals("success"))
    //             {
    //                 if (responseBody.InitiateChargeData.suggested_auth.Equals("PIN"))
    //                 {
    //                     var finalTransactionDetails =
    //                         Mapper.Map<TransactionDetails, FinalRequestPayload>(transactionDetails);
    //                     finalTransactionDetails.pin = pin;
    //                     finalTransactionDetails.suggested_auth = responseBody.InitiateChargeData.suggested_auth;
    //                     var chargeInitiatedEncryptionString = TdesEncryption.Encrypt(JsonConvert.SerializeObject(finalTransactionDetails), tdesKey);
    //
    //                     var finalPayload = new InitiatePaymentRequest
    //                     {
    //                         PBFPubKey = transactionDetails.PBFPubKey,
    //                         client = chargeInitiatedEncryptionString,
    //                     };
    //                     
    //                     var chargeInitiatedContent = new StringContent(JsonConvert.SerializeObject(finalPayload), Encoding.UTF8, "application/json");
    //                     var chargePaymentResult = await httpClient.PostAsync(raveUrl, chargeInitiatedContent);
    //
    //                     var chargeResponseJson =
    //                         (ChargeResponse) JsonConvert.DeserializeObject(await chargePaymentResult.Content
    //                             .ReadAsStringAsync());
    //
    //                     if (chargeResponseJson != null)
    //                     {
    //                         return chargeResponseJson;
    //                     }
    //                 }
    //             }
    //
    //         }
    //         catch (Exception e)
    //         {
    //             Console.WriteLine(e.Message);
    //         }
    //         return null;
    //     }
    //
    //     public Task<bool> ValidatePayment(ChargeResponse chargeResponse)
    //     {
    //         throw new NotImplementedException();
    //     }
    //
    //     public async Task VerifyPayment(InitiateFinalResponse response, TransactionDetails transactionDetails)
    //     {
    //     }
    // }
}