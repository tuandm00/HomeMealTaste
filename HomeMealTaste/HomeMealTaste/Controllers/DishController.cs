using HomeMealTaste.Models;
using HomeMealTaste.Services.Interface;
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

        public async Task<Dish> CreateDish(Dish dish)
        {
            var result = await _dishService.CreateDish(dish);
            return result;
        } 
        
    }
}
