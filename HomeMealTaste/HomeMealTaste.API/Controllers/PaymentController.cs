using HomeMealTaste.API.Dto;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Implement;
using HomeMealTaste.Services.Interface;
using HomeMealTaste.Services.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeMealTaste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class PaymentController : ControllerBase
    {
        //private readonly VnPayApiService _vnPayApiService;


        //public PaymentController(VnPayApiService vnPayApiService)
        //{
        //    _vnPayApiService = vnPayApiService;
        //}

        //[HttpPost("createpaymentlink")]
        //public ActionResult<string> CreatePaymentLink()
        //{
        //    try
        //    {
        //        // Validate and process the request, retrieve order information, and calculate the total amount

        //        // Example: Generating VNPay payment link
        //        string paymentLink = _vnPayApiService.GeneratePaymentLink();

        //        return Ok(new { PaymentLink = paymentLink });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the error or handle it as needed
        //        return BadRequest(new { Message = "Error creating payment link", Error = ex.Message });
        //    }
        //}

        private readonly IVnPayService _vnPayService;
        private readonly IConfiguration _configuration;

        public PaymentController(IVnPayService vnPayService, IConfiguration configuration)
        {
            _vnPayService = vnPayService;
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult CreatePaymentUrl(RechargeToWalletByVNPayRequestModel model)
        {
            var url = _vnPayService.CreateRechargeUrlForWallet(model);

            return new ObjectResult(url);
        }

        [HttpGet("get-payment-return")]
        public async Task<IActionResult> PaymentCallback()
        {
            var response = _vnPayService.PaymentExcute(Request.Query);

            return Ok(response);
        }
    }
}


