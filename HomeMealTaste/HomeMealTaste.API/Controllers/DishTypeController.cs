using HomeMealTaste.Data.Helper;
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
    
    public class DishTypeController : ControllerBase
    {
        private readonly IDishTypeService _dishTypeServices;

        public DishTypeController(IDishTypeService dishTypeServices)
        {

            _dishTypeServices = dishTypeServices;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDishType(DishTypeRequestModel dishTypeRequest)
        {
            var result = await _dishTypeServices.CreateDishType(dishTypeRequest);
            return Ok(result);
        }

        [HttpGet("get-all-dish-type")]
        public async Task<ApiResponse<PagedList<DishType>>> GetAllDish([FromQuery] PagingParams pagingParams)
        {
            var result = await _dishTypeServices.GetAllDishType(pagingParams);
            var metadata = new
            {
                result.TotalCount,
                result.TotalPages,
                result.PageSize,
                result.CurrentPage,
                result.HasNext,
                result.HasPrevious
            };
            return ApiResponse<List<DishType>>.Success(result, metadata);
        }   
        
        [HttpDelete]
        public Task DeleteDishTypeById(int id)
        {
            var result = _dishTypeServices.DeleteDishTypeById(id);
            return result;
        }
        [HttpPut("update-dishtype")]
        public async Task<IActionResult> UpdateDishType(UpdateDishTypeRequestModel request)
        {
            var result = await _dishTypeServices.UpdateDishType(request);
            return Ok(result);
        }
        [HttpGet("get-single-dish-type")]
        public async Task<IActionResult> GetSingleDishTypeById(int dishtypeId)
        {
            var result = await _dishTypeServices.GetSingleDishTypeById(dishtypeId);
            return Ok(result);
        }
    }
}
