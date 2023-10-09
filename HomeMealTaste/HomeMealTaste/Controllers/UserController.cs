using HomeMealTaste.Data.Models;
using HomeMealTaste.Services.ResponseModel;
using HomeMealTaste.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HomeMealTaste.Data.RequestModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;

namespace HomeMealTaste.Controllers
{
    [Authorize()]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserRequestModel userRequest)
        {
            var result = await _userService.LoginAsync(userRequest);
            return Ok(result);
        }

        [HttpPost]
        [Route("registerforcustomer")]
        public async Task<IActionResult> RegisterForCustomer(User user)
        {
            var result = await _userService.RegisterForCustomer(user);
            return Ok(result);
        }

        [HttpPost]
        [Route("registerforchef")]
        public async Task<IActionResult> RegisterForChef(User user)
        {
            var result = await _userService.RegisterForChef(user);
            return Ok(result);
        }

        [HttpDelete]
        [Route("deleteuserbyid")]
        public async Task<IActionResult> DeleteUserById(int id)
        {
            var result = await _userService.DeleteUserById(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("getalluser")]
        public List<User> GetAllUser() => _userService.GetAllUser();

        [HttpPatch]
        [Route("forgetpassword")]
        public async Task<IActionResult> ForgetPassword(string user)
        {
            var result = await _userService.ForgetPassword(user);
            return Ok(result);
        }

    }
}
