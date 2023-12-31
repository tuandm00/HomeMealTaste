﻿using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Response;
using HomeMealTaste.Services.Helper;
using HomeMealTaste.Services.Interface;
using HomeMealTaste.Services.ResponseModel;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> CreateSessionWithDay(SessionRequestModel sessionRequest)
        {
            var result = await _sessionService.CreateSessionWithDay(sessionRequest);
            return Ok(result);
        }

        //[HttpPatch("update-end-time-session")]
        //public async Task<SessionResponseModel> UpdateEndTime(int sessionId, DateTime endTime)
        //{
        //    var result = await _sessionService.UpdateEndTime(sessionId, endTime);
        //    return result;
        //}

        //[HttpGet("get-all")]
        //public async Task<IActionResult> GetAllMealInSession([FromQuery] GetAllMealRequest pagingParams)
        //{
        //    var result = await _sessionService.GetAllMealInCurrentSession(pagingParams);
        //    var metadata = new
        //    {
        //        result.TotalCount,
        //        result.TotalPages,
        //        result.PageSize,
        //        result.CurrentPage,
        //        result.HasNext,
        //        result.HasPrevious
        //    };
        //    var response = ApiResponse<object>.Success(result, metadata);
        //    return Ok(response);
        //}

        [HttpPatch("change-status-session")]
        public async Task ChangeStatusSession(ChangeStatusSessionRequestModel request, bool status)
        {
            await _sessionService.ChangeStatusSession(request, status);
            
        }

        [HttpGet("get-all-session")]
        public async Task<IActionResult> GetAllSession()
        {
            var result = await _sessionService.GetAllSession();
            return Ok(result);
        }
        [HttpGet("get-all-session-by-area-id")]
        public async Task<IActionResult> GetAllSessionByAreaId(int areaid)
        {
            var result = await _sessionService.GetAllSessionByAreaId(areaid);
            return Ok(result);
        }
        [HttpGet("get-all-session-by-area-id-with-status-true")]
        public async Task<IActionResult> GetAllSessionByAreaIdWithStatusTrue(int areaid)
        {
            var result = await _sessionService.GetAllSessionByAreaIdWithStatusTrue(areaid);
            return Ok(result);
        }

        //[HttpDelete]
        //public async Task<IActionResult> DeleteSession(int sessionid)
        //{
        //    var result = _sessionService.DeleteSession(sessionid);
        //    return Ok(result);
        //}
        [HttpGet("get-single-session-by-session-id")]
        public async Task<IActionResult> GetSingleSessionBySessionId(int sessionid)
        {
            var result = await _sessionService.GetSingleSessionBySessionId(sessionid);
            return Ok(result);
        }
        [HttpGet("get-all-session-with-status-on")]
        public async Task<IActionResult> GetAllSessionStatusOn()
        {
            var result = await _sessionService.GetAllSessionStatusOn();
            return Ok(result);
        }
        [HttpPatch("change-status-register-for-meal")]
        public async Task ChangeStatusRegisterForMeal(ChangeStatusRegisterForMealRequestModel request)
        {
            await _sessionService.ChangeStatusRegisterForMeal(request);
        }
        [HttpPatch("change-status-booking-slot")]
        public async Task ChangeStatusBookingSlot(ChangeStatusBookingSlotRequestModel request)
        {
            await _sessionService.ChangeStatusBookingSlot(request);
        }
        [HttpPut("update-session-and-area-in-session")]
        public async Task<IActionResult> UpdateSessionAndAreaInSession(UpdateSessionAndAreaInSessionRequestModel request)
        {
            var result = await _sessionService.UpdateSessionAndAreaInSession(request);
            return Ok(result);
        }

        //[HttpGet("get-all-session-by-area-id-with-status-true-in-day")]
        //public async Task<IActionResult> GetAllSessionByAreaIdWithStatusTrueInDay(int areaid)
        //{
        //    var result = await _sessionService.GetAllSessionByAreaIdWithStatusTrueInDay(areaid);
        //    return Ok(result);

        //}

        [HttpGet("get-all-session-with-status-true-and-booking-slot-true")]
        public async Task<IActionResult> GetAllSessionWithStatusTrueAndBookingSlotTrue()
        {
            var result = await _sessionService.GetAllSessionWithStatusTrueAndBookingSlotTrue();
            return Ok(result);

        }
        
    }
}
