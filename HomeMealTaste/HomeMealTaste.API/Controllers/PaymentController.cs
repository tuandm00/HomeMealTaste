using HomeMealTaste.API.Dto;
using HomeMealTaste.Services.Implement;
using HomeMealTaste.Services.Interface;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HomeMealTaste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly VnPayApiService _vnPayApiService;


        public PaymentController(VnPayApiService vnPayApiService)
        {
            _vnPayApiService = vnPayApiService;
        }

        [HttpPost("createpaymentlink")]
        public ActionResult<string> CreatePaymentLink()
        {
            try
            {
                // Validate and process the request, retrieve order information, and calculate the total amount

                // Example: Generating VNPay payment link
                string paymentLink = _vnPayApiService.GeneratePaymentLink();

                return Ok(new { PaymentLink = paymentLink });
            }
            catch (Exception ex)
            {
                // Log the error or handle it as needed
                return BadRequest(new { Message = "Error creating payment link", Error = ex.Message });
            }
        }
        
    }
}


