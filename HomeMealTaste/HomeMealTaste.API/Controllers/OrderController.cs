﻿using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Helper;
using HomeMealTaste.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeMealTaste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("get-order-by-user-id")]
        public async Task<IActionResult> GetAllOrderByUserId(int id)
        {
            var result = await _orderService.GetAllOrderByUserId(id);
            return Ok(result);
        }

        [HttpGet("get-all-order")]
        public async Task<IActionResult> GetAllOrder()
        {
            var result = await _orderService.GetAllOrder();
            return Ok(result);
        }

        [HttpGet("get-order-by-order-id")]
        public async Task<IActionResult> GetAllOrderById(int id)
        {
            var result = await _orderService.GetAllOrderById(id);
            return Ok(result);
        }

        [HttpGet("get-order-by-kitchen-id")]
        public async Task<IActionResult> GetOrderByKitchenId(int kitchenid)
        {
            var result = await _orderService.GetOrderByKitchenId(kitchenid);
            return Ok(result);
        }

    }
}
