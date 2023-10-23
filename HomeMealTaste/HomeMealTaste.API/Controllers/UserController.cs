using HomeMealTaste.Data.Helper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Response;
using HomeMealTaste.Services.Helper;


namespace HomeMealTaste.Controllers
{
    
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
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserRequestModel userRequest)
        {
            var result = await _userService.LoginAsync(userRequest);
            return Ok(result);
        }

        [HttpPost("register-for-customer")]
        public async Task<IActionResult> RegisterForCustomer(UserRegisterCustomerRequestModel userRegisterCustomerRequest)
        {
            var result = await _userService.RegisterForCustomer(userRegisterCustomerRequest);
            return Ok(result);
        }


        [HttpPost("register-for-chef")]
        public async Task<IActionResult> RegisterForChef(UserRegisterChefRequestModel userRegisterChefRequest)
        {
            var result = await _userService.RegisterForChef(userRegisterChefRequest);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUserById(int id)
        {
            var result = await _userService.DeleteUserById(id);
            return Ok(result);
        }

        [HttpGet("get-all-user")]
        public async Task<ApiResponse<PagedList<User>>> GetAllUser([FromQuery] PagingParams pagingParams)
        {
            var result = await _userService.GetAllUser(pagingParams);
            var metadata = new
            {
                result.TotalCount,
                result.TotalPages,
                result.PageSize,
                result.CurrentPage,
                result.HasNext,
                result.HasPrevious
            };
            return ApiResponse<List<User>>.Success(result, metadata);
        }

        [HttpPatch("forget-password")]
        public async Task<IActionResult> ForgetPassword(string username)
        {
            var result = await _userService.ForgetPassword(username);
            return Ok(result);
        }

        [HttpPut("update-account-for-user")]
        public async Task UpdatePasswordAccount(string username, string newPassword)
        {
            await _userService.UpdatePasswordAccount(username,newPassword);
        }
    }
}
