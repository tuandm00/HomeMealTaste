using HomeMealTaste.Data.Helper;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Response;
using HomeMealTaste.Services.Helper;
using HomeMealTaste.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeMealTaste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KitchenController : ControllerBase
    {
        private readonly IKitchenService _kitchenService;

        public KitchenController(IKitchenService kitchenService)
        {
            _kitchenService = kitchenService;
        }

        [HttpGet("get-all-kitchen")]
        public async Task<IActionResult> GetAllKitchen()
        {
            var result = await _kitchenService.GetAllKitchen();
            return Ok(result);
        }
        [HttpGet("get-all-kitchen-by-kitchen-id")]
        public async Task<IActionResult> GetAllKitchenByKitchenId(int id)
        {
            var result = await _kitchenService.GetAllKitchenByKitchenId(id);
            return Ok(result);
        }
        
        [HttpGet("get-all-kitchen-by-session-id")]
        public async Task<IActionResult> GetAllKitchenBySessionId(int sessionid)
        {
            var result = await _kitchenService.GetAllKitchenBySessionId(sessionid);
            return Ok(result);
        }
        
        
    }
}
