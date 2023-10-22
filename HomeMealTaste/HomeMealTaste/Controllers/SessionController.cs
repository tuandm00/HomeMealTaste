using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.RequestModel;
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
        [Route("create-session")]
        public async Task<SessionResponseModel> CreateSession(SessionRequestModel sessionRequest)
        {
            var result = await _sessionService.CreateSession(sessionRequest);
            return result;
        }

        [HttpPatch]
        [Route("update-end-time-session")]

        public async Task<SessionResponseModel> UpdateEndTime(int sessionId, DateTime endTime)
        {
            var result = await _sessionService.UpdateEndTime(sessionId, endTime);
            return result;
        }
    }
}
