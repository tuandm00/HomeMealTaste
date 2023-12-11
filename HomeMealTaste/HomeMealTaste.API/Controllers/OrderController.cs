using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Helper;
using HomeMealTaste.Services.Implement;
using HomeMealTaste.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace HomeMealTaste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly HomeMealTasteContext _context;
        private readonly IHubContext<OrderNotificationHub> _hubContext;
        public OrderController(IOrderService orderService, IHubContext<OrderNotificationHub> hubContext, HomeMealTasteContext context)
        {
            _orderService = orderService;
            _hubContext = hubContext;
            _context = context;
        }

        [HttpGet("get-order-by-customer-id")]
        public async Task<IActionResult> GetAllOrderByCustomerId(int id)
        {
            var result = await _orderService.GetAllOrderByCustomerId(id);
            return Ok(result);
        }

        [HttpGet("get-all-order")]
        public async Task<IActionResult> GetAllOrder()
        {
            var result = await _orderService.GetAllOrder();
            return Ok(result);
        }

        [HttpGet("get-order-by-order-id")]
        public async Task<IActionResult> GetSingleOrderById(int id)
        {
            var result = await _orderService.GetSingleOrderById(id);
            return Ok(result);
        }

        [HttpGet("get-order-by-kitchen-id")]
        public async Task<IActionResult> GetOrderByKitchenId(int kitchenid)
        {
            var result = await _orderService.GetOrderByKitchenId(kitchenid);
            return Ok(result);
        }
        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder(CreateOrderRequestModel createOrderRequest)
        {
            var result = _orderService.CreateOrder(createOrderRequest);
            return Ok(result);
        }
        [HttpPost("refund-money-when-chef-order-cancel")]
        public Task ChefCancelledOrderRefundMoneyToCustomerV2(int mealsessionId)
        {
            var result = _orderService.ChefCancelledOrderRefundMoneyToCustomerV2(mealsessionId);
            return result;
        }
        [HttpPatch("change-status-order")]
        public async Task<IActionResult> ChangeStatusOrder(int mealsessionid, string status)
        {
            var result = await _orderService.ChangeStatusOrder(mealsessionid, status);
            return Ok(result);
        }
        [HttpGet("count-order-in-system")]
        public async Task<IActionResult> TotalOrderInSystem()
        {
            var result = await _orderService.TotalOrderInSystem();
            return Ok(result);
        }
        [HttpGet("get-all-order-by-mealsession-id")]
        public async Task<IActionResult> GetAllOrderByMealSessionId(int mealsessionid)
        {
            var result = await _orderService.GetAllOrderByMealSessionId(mealsessionid);
            return Ok(result);
        }
        [HttpGet("get-total-price-of-mealsession-by-meal-session-id")]
        public async Task<IActionResult> GetTotalPriceWithMealSessionByMealSessionId(int mealsessionid)
        {
            var result = await _orderService.GetTotalPriceWithMealSessionByMealSessionId(mealsessionid);
            return Ok(result);
        }
        [HttpGet("get-total-price-of-mealsession-by-session-id-and-kitchen-id")]
        public async Task<IActionResult> GetTotalPriceWithMealSessionBySessionIdAndKitchenId(int sessionId , int kitchenId)
        {
            var result = await _orderService.GetTotalPriceWithMealSessionBySessionIdAndKitchenId(sessionId, kitchenId);
            return Ok(result);
        }
        //[HttpGet("total-price-of-order-in-system-in-every-month")]
        //public async Task<IActionResult> TotalPriceOfOrderInSystemInEveryMonth(int month)
        //{
        //    var result = await _orderService.TotalPriceOfOrderInSystemInEveryMonth(month);
        //    return Ok(result);
        //}
        [HttpGet("total-price-of-order-in-system")]
        public async Task<IActionResult> TotalPriceOfOrderInSystemWithEveryMonth()
        {
            var result = await _orderService.TotalPriceOfOrderInSystemWithEveryMonth();
            return Ok(result);
        }
        [HttpGet("total-customer-order-in-system")]
        public async Task<IActionResult> TotalCustomerOrderInSystem()
        {
            var result = await _orderService.TotalCustomerOrderInSystem();
            return Ok(result);
        }
        [HttpGet("get-top-5-customer-order-times")]
        public async Task<IActionResult> GetTop5CustomerOrderTimes()
        {
            var result = await _orderService.GetTop5CustomerOrderTimes();
            return Ok(result);
        }
        [HttpGet("get-top-5-chef-order-times")]
        public async Task<IActionResult> GetTop5ChefOrderTimes()
        {
            var result = await _orderService.GetTop5ChefOrderTimes();
            return Ok(result);
        }

    }
}
