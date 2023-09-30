using HomeMealTaste.Data.Models;
using HomeMealTaste.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace HomeMealTaste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private IConfiguration _config;
        private readonly HomeMealTasteContext _ctx;
        private readonly IUserService _userService;
        public UserController(IConfiguration configuration, HomeMealTasteContext context, IUserService userService)
        {
            _config = configuration;
            _ctx = context;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(User users)
        {
            var result = await _userService.LoginAsync(users);
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
    }
}
