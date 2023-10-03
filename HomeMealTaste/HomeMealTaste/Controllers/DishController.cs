﻿using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.RequestModel;
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
        public  List<DishRequestModel> GetAllDish() => _dishService.GetAllDish();

        [HttpDelete]
        [Route("deletedishid")]
        public async Task<IActionResult> DeleteDishId(int id)
        {
            var result = await _dishService.DeleteDishId(id);
            return Ok(result);
        }        
    }
}
