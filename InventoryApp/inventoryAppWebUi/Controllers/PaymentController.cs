using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using inventoryAppDomain.Entities.Dtos;
using inventoryAppDomain.Services;
using inventoryAppWebUi.Models;

namespace inventoryAppWebUi.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        // GET
        public ActionResult Index(int orderId)
        {
            return View(new TransactionViewModel{OrderId = orderId});
        }

        public async Task<ActionResult> ProcessPayment(int orderId)
        {
            var result = await _paymentService.InitiatePayment(orderId);
            return Redirect(result.checkoutUrl);
        }

        public async Task<ActionResult> VerifyPayment(string paymentReference)
        {
            var response = await _paymentService.VerifyPayment(paymentReference);
            Console.WriteLine(response);
            if (response)
            {
                ViewBag.PaymentResponse = true;
            }
            else
            {
                ViewBag.PaymentResponse = false;
            }
            return RedirectToAction("Index", "Home");
        }
    }
}