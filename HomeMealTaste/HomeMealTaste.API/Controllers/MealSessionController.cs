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
        public async Task<ApiResponse<PagedList<MealSession>>> GetAllUser([FromQuery] PagingParams pagingParams)
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
            return ApiResponse<List<User>>.Success(result, metadata);
        }
    }
}
