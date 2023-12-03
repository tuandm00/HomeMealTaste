using HomeMealTaste.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeMealTaste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet("get-revenue-in-system")]
        public async Task<IActionResult> GetRevenueSystem()
        {
            var result = _walletService.GetRevenueSystem();
            return Ok(result);
        }
    }
}
