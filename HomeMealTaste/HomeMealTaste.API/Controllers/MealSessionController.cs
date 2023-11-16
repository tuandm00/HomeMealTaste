﻿using FirebaseAdmin.Messaging;
using HomeMealTaste.Data.Helper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Response;
using HomeMealTaste.Services.Helper;
using HomeMealTaste.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HomeMealTaste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealSessionController : ControllerBase
    {
        private readonly IMealSessionService _mealSessionService;

        public MealSessionController(IMealSessionService mealSessionService)
        {
            _mealSessionService = mealSessionService;
        }

        [HttpPost]
        public async Task<MealSessionResponseModel> CreateMealSession(MealSessionRequestModel mealSessionRequest)
        {
            var result = await _mealSessionService.CreateMealSession(mealSessionRequest);
            return result;
        }
        
        [HttpGet("get-all-meal-sessions")]
        public async Task<IActionResult> GetAllUser([FromQuery] GetAllMealRequest pagingParams)
        {
            var result = await _mealSessionService.GetAllMealSession(pagingParams);
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
        [HttpPost("sendNotification")]
        //title = push status for mealsession, body =status
        public async Task SendNotificationAsync(string DeviceToken, string title, string body)
        {
            var message = new Message()
            {
                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Title = title,
                    Body = body
                },
                Token = DeviceToken,
            };
            var messaging = FirebaseMessaging.DefaultInstance;
            var result = await messaging.SendAsync(message);
        }
    }
}
