﻿using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeMealTaste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class DistrictController : ControllerBase
    {
        private readonly IDistrictService _districtService;

        public DistrictController(IDistrictService districtService)
        {
            _districtService = districtService;
        }

        [HttpPost]
        [Route("create-district")]
        public async Task<IActionResult> CreateDistrict(DistrictRequestModel districtRequest)
        {
            var result = await _districtService.CreateDistrict(districtRequest);
            return Ok(result);
        }

        [HttpGet("get-all-district")]
        public async Task<IActionResult> GetAlllDistrict()
        {
            var result =  _districtService.GetAllDistrict();
            return Ok(result);
        }     

        [HttpPut("update-district")]
        public async Task<IActionResult> UpdateDistrict(UpdateDistrictRequestModel districtRequest)
        {
            var result = await _districtService.UpdateDistrict(districtRequest);
            return Ok(result);
        }   
        [HttpDelete("delete-district")]
        public async Task<IActionResult> DeleteDistrict(int districtId)
        {
            var result = await _districtService.DeleteDistrict(districtId);
            return Ok(result);
        }     
    }
}
