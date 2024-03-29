﻿using HomeMealTaste.Data.RequestModel;
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
        public async Task ChangeStatusSessionArea(List<int> sessionArea, string status)
        {
            await _sessionAreaService.ChangeStatusSessionArea(sessionArea, status);

        }
        [HttpGet("get-all-session-area-by-session-id")]
        public async Task<IActionResult> GetAllSessionAreaBySessionId(int sessionId)
        {
            var result = await _sessionAreaService.GetAllSessionAreaBySessionId(sessionId);
            return Ok(result);

        }

        [HttpGet("get-single-session-area-by-session-area-id")]
        public async Task<IActionResult> GetSingleSessionAreaBySessionAreaId(int sessionAreaId)
        {
            var result = await _sessionAreaService.GetSingleSessionAreaBySessionAreaId(sessionAreaId);
            return Ok(result);

        }

    }
}
