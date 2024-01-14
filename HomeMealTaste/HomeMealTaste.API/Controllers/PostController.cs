using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeMealTaste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly INotificationService _notificationService;


        public PostController(IPostService postService, INotificationService notificationService)
        {
            _postService = postService;
            _notificationService = notificationService;
        }

        [HttpPost("post-a-notification-by-email")]
        public async Task PostForAllCustomerWithOrderId(int mealsessionId)
        {
            var result = _postService.PostForAllCustomerWithOrderId(mealsessionId);
           
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendNotificationForMealSession(int mealSessionId, string Status)
        {
            var result = await _notificationService.SendNotificationForMealSession(mealSessionId, Status);
            return Ok(result);
        }
    }
}
