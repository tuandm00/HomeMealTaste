using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeMealTaste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }
        [HttpPost("create-feedback")]
        public async Task<IActionResult> CreateFeedback(FeedbackRequestModel feedbackRequestModel)
        {
            var result = await _feedbackService.CreateFeedback(feedbackRequestModel);
            return Ok(result);
        }
        [HttpGet("get-all-feedback")]
        public async Task<IActionResult> GetAllFeedback()
        {
            var result = await _feedbackService.GetAllFeedback();
            return Ok(result);
        }
    }
}
