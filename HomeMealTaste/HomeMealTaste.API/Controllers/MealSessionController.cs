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

        //[HttpGet("get-all-meal-sessions")]
        //public async Task<IActionResult> GetAllUser([FromQuery] GetAllMealRequest pagingParams)
        //{
        //    var result = await _mealSessionService.GetAllMealSession(pagingParams);
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

        [HttpGet("get-all-meal-session")]
        public async Task<IActionResult> GetAllMealSession()
        {
            var result = await _mealSessionService.GetAllMealSession();
            return Ok(result);
        }
        [HttpGet("get-single-meal-session-by-meal-session-id")]
        public async Task<IActionResult> GetSingleMealSessionById(int mealsessionid)
        {
            var result = await _mealSessionService.GetSingleMealSessionById(mealsessionid);
            return Ok(result);
        }
        [HttpGet("get-all-meal-session-by-status")]
        public async Task<IActionResult> GetAllMealSessionByStatus(string status)
        {
            var result = await _mealSessionService.GetAllMealSessionByStatus(status);
            return Ok(result);
        }
        [HttpPatch("update-status-meal-session")]
        public Task UpdateStatusMeallSession(int mealSessionid, string status)
        {
            var result = _mealSessionService.UpdateStatusMeallSession(mealSessionid, status);
            return result;
        }
        [HttpGet("get-all-meal-session-by-session-id-IN-DAY")]
        public async Task<IActionResult> GetAllMeallSessionBySessionIdINDAY(int sessionid)
        {
            var result = await _mealSessionService.GetAllMeallSessionBySessionIdINDAY(sessionid);
            return Ok(result);
        }
        [HttpGet("get-all-meal-session-by-session-id")]
        public async Task<IActionResult> GetAllMeallSessionBySessionId(int sessionid)
        {
            var result = await _mealSessionService.GetAllMeallSessionBySessionId(sessionid);
            return Ok(result);
        }

        [HttpGet("get-all-meal-session-by-kitchen-id")]
        public async Task<IActionResult> GetAllMeallSessionByKitchenId(int kitchenId)
        {
            var result = await _mealSessionService.GetAllMeallSessionByKitchenId(kitchenId);
            return Ok(result);
        }
        [HttpGet("get-all-meal-session-with-status-APPROVED-and-REMAINQUANTITY->-0-IN-DAY")]
        public async Task<IActionResult> GetAllMeallSessionWithStatusAPPROVEDandREMAINQUANTITYhigherthan0InDay()
        {
            var result = await _mealSessionService.GetAllMeallSessionWithStatusAPPROVEDandREMAINQUANTITYhigherthan0InDay();
            return Ok(result);
        }
        [HttpGet("get-all-meal-session-by-kitchen-id-in-session")]
        public async Task<IActionResult> GetAllMeallSessionByKitchenIdInSession(int kitchenid, int sessionid)
        {
            var result = await _mealSessionService.GetAllMeallSessionByKitchenIdInSession(kitchenid, sessionid);
            return Ok(result);
        }
        [HttpGet("get-all-meal-session-by-session-id-with-status-approved-and-remain-quantity->0-IN-DAY")]
        public async Task<IActionResult> GetAllMeallSessionBySessionIdWithStatusApprovedandREMAINQUANTITYhigherthan0InDay(int sessionid)
        {
            var result = await _mealSessionService.GetAllMeallSessionBySessionIdWithStatusApprovedandREMAINQUANTITYhigherthan0InDay(sessionid);
            return Ok(result);
        } 
        [HttpGet("get-all-meal-session-by-kitchen-id-with-status-approved-and-remain-quantity->0-IN-DAY")]
        public async Task<IActionResult> GetAllMeallSessionByKitchenIdWithStatusApprovedandREMAINQUANTITYhigherthan0InDay(int kitchenid)
        {
            var result = await _mealSessionService.GetAllMeallSessionByKitchenIdWithStatusApprovedandREMAINQUANTITYhigherthan0InDay(kitchenid);
            return Ok(result);
        }
    }
}

