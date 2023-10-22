﻿using HomeMealTaste.Data.Models;
using HomeMealTaste.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HomeMealTaste.Data.RequestModel;


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
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserRequestModel userRequest)
        {
            var result = await _userService.LoginAsync(userRequest);
            return Ok(result);
        }

        [HttpPost]
        [Route("registerforcustomer")]
        public async Task<IActionResult> RegisterForCustomer(UserRegisterCustomerRequestModel userRegisterCustomerRequest)
        {
            var result = await _userService.RegisterForCustomer(userRegisterCustomerRequest);
            return Ok(result);
        }

        [HttpPost]
        [Route("registerforchef")]
        public async Task<IActionResult> RegisterForChef(UserRegisterChefRequestModel userRegisterChefRequest)
        {
            var result = await _userService.RegisterForChef(userRegisterChefRequest);
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
        public async Task<IActionResult> ForgetPassword(string username)
        {
            var result = await _userService.ForgetPassword(username);
            return Ok(result);
        }

        [HttpPut]
        [Route("updateaccountforuser")]
        public async Task UpdatePasswordAccount(string username, string newPassword)
        {
            await _userService.UpdatePasswordAccount(username,newPassword);
        }
    }
}
