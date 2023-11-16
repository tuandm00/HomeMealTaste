using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Response;
using HomeMealTaste.Services.Helper;
using HomeMealTaste.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeMealTaste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealController : ControllerBase
    {
        private readonly IMealService _mealService;
        public MealController(IMealService mealService)
        {
            _mealService = mealService;
        }

        [HttpPost]
        [Route("create-meal")]
        public async Task<IActionResult> CreateMeal([FromForm]MealRequestModel mealRequest)
        {
            var result = await _mealService.CreateMeal(mealRequest);
            return Ok(result);
        }
        
        [HttpGet("get-all")]
        //public async Task<IActionResult> GetAllMeal([FromQuery] PagingParams pagingParams)
        //{
        //    var result = await _mealService.GetAllMeal(pagingParams);
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

        [HttpGet("get-all-meal-by-kitchen-id")]
        public async Task<IActionResult> GetAllMealByKitchenId(int id)
        {
            var result = await _mealService.GetAllMealByKitchenId(id);
            return Ok(result);
        }

        [HttpGet("get-single-meal-by-meal-id")]
        public async Task<IActionResult> GetMealByMealId(int mealid)
        {
            var result = await _mealService.GetMealByMealId(mealid);
            return Ok(result);
        }

    }
}
