using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Response;
using HomeMealTaste.Services.Helper;
using HomeMealTaste.Services.Interface;
using HomeMealTaste.Services.ResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeMealTaste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpPost]
        public async Task<SessionResponseModel> CreateSession(SessionRequestModel sessionRequest)
        {
            var result = await _sessionService.CreateSession(sessionRequest);
            return result;
        }

        [HttpPatch("update-end-time-session")]
        public async Task<SessionResponseModel> UpdateEndTime(int sessionId, DateTime endTime)
        {
            var result = await _sessionService.UpdateEndTime(sessionId, endTime);
            return result;
        }
        
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllMealInSession([FromQuery] GetAllMealRequest pagingParams)
        {
            var result = await _sessionService.GetAllMealInCurrentSession(pagingParams);
            var metadata = new
            {
                result.TotalCount,
                result.TotalPages,
                result.PageSize,
                result.CurrentPage,
                result.HasNext,
                result.HasPrevious
            };
            var response = ApiResponse<object>.Success(result, metadata);
            return Ok(response);
        }
    }
}
