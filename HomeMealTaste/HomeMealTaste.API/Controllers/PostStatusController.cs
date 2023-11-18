using FirebaseAdmin.Messaging;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeMealTaste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostStatusController : ControllerBase
    {
        private readonly IPostService _postService;
        public PostStatusController(IPostService postService)
        {
            _postService = postService;
        }
        [HttpPost("create-post-status")]
        public async Task<IActionResult> CreatePostStatus(PostRequestModel postRequestModel)
        {
            var result= await _postService.CreatePostStatusAfterOrder(postRequestModel);
            var status = postRequestModel.Status;
            var orderId = postRequestModel.OrderId;
            var nameOfMeal = _postService.getMealNameByOrderId((int)orderId);
            await Console.Out.WriteLineAsync(await nameOfMeal);
            //await SendNotificationAsync("", nameOfMeal, status);

            return Ok(result);
        }
        [HttpPost("sendNotification")]
        public async Task SendNotificationAsync(string DeviceToken, Task<string> title, string? body)
        {
            var message = new Message()
            {
                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Title ="Your Meal:"+ title +"is",
                    Body = body
                },
                Token = DeviceToken,
            };
            var messaging = FirebaseMessaging.DefaultInstance;
            var result = await messaging.SendAsync(message);
        }

    }
}
