using HomeMealTaste.Data.Helper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Response;
using HomeMealTaste.Services.Helper;
using HomeMealTaste.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HomeMealTaste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateDishAsync([FromForm]DishRequestModel dishRequest)
        {
            var result = await _dishService.CreateDishAsync(dishRequest);
            return Ok(result);
        }

        [HttpGet("get-all-dish")]
        public async Task<ApiResponse<PagedList<Dish>>> GetAllDishAsync([FromQuery] PagingParams pagingParams)
        {
            var result = await _dishService.GetAllDishAsync(pagingParams);
            var metadata = new
            {
                result.TotalCount,
                result.TotalPages,
                result.PageSize,
                result.CurrentPage,
                result.HasNext,
                result.HasPrevious
            };
            return ApiResponse<List<Dish>>.Success(result, metadata);
        } 

        [HttpDelete]
        public async Task<IActionResult> DeleteDishAsync(int id)
        {
            await _dishService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDetailAsync(int id)
        {
            var result = await _dishService.GetDetailAsync(id);

            return Ok(result);
        }

        [HttpGet("get-dish-id-by-meal-id")]
        public async Task<IActionResult> GetDishIdByMealId(int mealid)
        {
            var result = await _dishService.GetDishIdByMealId(mealid);
            return Ok(result);
        }
        [HttpGet("get-dish-by-kitchen-id")]
        public async Task<IActionResult> GetDishByKitchenId(int kitchenid)
        {
            var result = await _dishService.GetDishByKitchenId(kitchenid);
            return Ok(result);
        }

    }
}
