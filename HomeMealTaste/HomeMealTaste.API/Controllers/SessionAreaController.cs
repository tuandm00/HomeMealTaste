using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeMealTaste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionAreaController : ControllerBase
    {
        private readonly ISessionAreaService _sessionAreaService;

        public SessionAreaController(ISessionAreaService sessionAreaService)
        {
            _sessionAreaService = sessionAreaService;
        }

        [HttpGet("get-all-session-area")]
        public async Task<IActionResult> GetAllSessionArea()
        {
            var result = await _sessionAreaService.GetAllSessionArea();
            return Ok(result);
        }
        [HttpPatch("update-status-session-area")]
        public async Task<IActionResult> UpdateStatusSessionArea(UpdateStatusSessionAreaRequestModel request)
        {
            var result = await _sessionAreaService.UpdateStatusSessionArea(request);
            return Ok(result);
        }
        [HttpPatch("change-status-session-area")]
        public async Task ChangeStatusSessionArea(int sessionId)
        {
             await _sessionAreaService.ChangeStatusSessionArea(sessionId);
            
        }
        
    }
}
