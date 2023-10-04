using HomeMealTaste.Data.RequestModel;
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
        [Route("createdishtype")]
        public async Task<DishTypeResponseModel> CreateDishType(DishTypeRequestModel dishTypeRequest)
        {
            var result = await _dishTypeServices.CreateDishType(dishTypeRequest);
            return result;
        }

        [HttpGet]
        [Route("getalldishtype")]
        public List<DishTypeRequestModel> GetAllDishType() => _dishTypeServices.GetAllDishType();   
    }
}
