using HomeMealTaste.Data.Helper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Response;
using HomeMealTaste.Services.Helper;
using HomeMealTaste.Services.Interface;
using HomeMealTaste.Services.ResponseModel;
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
        public async Task<DishTypeResponseModel> CreateDishType(DishTypeRequestModel dishTypeRequest)
        {
            var result = await _dishTypeServices.CreateDishType(dishTypeRequest);
            return result;
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
    }
}
