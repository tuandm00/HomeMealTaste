using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Helper;
using HomeMealTaste.Services.Implement;
using HomeMealTaste.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace HomeMealTaste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        [HttpPost("CompleteOrder/{orderId}")]
        public async Task<IActionResult> CompleteOrder(int orderId)
        {
            try
            {
                // Retrieve the order from the database
                var order = _context.Orders
                    .Include(x => x.Customer)
                    .FirstOrDefault(x => x.OrderId == orderId);

                if (order == null)
                {
                    return NotFound($"Order with ID {orderId} not found.");
                }

                // Logic to mark the order as completed by the chef
                order.Status = "PAID"; // Replace OrderStatus with your actual status enum

                // Save changes to the database
                _context.SaveChanges();

                // Notify the customer using SignalR
                var customerId = order.Customer.UserId; // Assuming Customer has a UserId property
                var message = "Your order is ready!"; // You can customize the message

                await _hubContext.Clients.User(customerId.ToString()).SendAsync("ReceiveOrderNotification", message);

                return Ok($"Order with ID {orderId} completed successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it according to your application's needs
                return StatusCode(500, "An error occurred while completing the order.");
            }
        }
    }
}
