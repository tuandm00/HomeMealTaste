using HomeMealTaste.Data.Models;
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
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }

        [Authorize]
        [HttpPost]
        [Route("adddish")]

        public async Task<DishResponseModel> CreateDish(DishRequestModel dishRequest)
        {
            var result = await _dishService.CreateDish(dishRequest);
            return result;
        }

        [HttpGet]
        [Route("getalldish")]
        public async Task<ApiResponse<PagedList<Dish>>> GetAllDish([FromQuery] PagingParams pagingParams)
        {
            var result = await _dishService.GetAllDish(pagingParams);
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
        [Route("deletedishid")]
        public async Task<IActionResult> DeleteDishId(int id)
        {
            var result = await _dishService.DeleteDishId(id);
            return Ok(result);
        }        
    }
}
