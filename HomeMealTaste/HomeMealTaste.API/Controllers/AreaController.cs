﻿using HomeMealTaste.Data.Helper;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Response;
using HomeMealTaste.Services.Helper;
using HomeMealTaste.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeMealTaste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AreaController : ControllerBase
    {
        private readonly IAreaService _areaService;

        public AreaController(IAreaService areaService)
        {
            _areaService = areaService;
        }

        [HttpGet("get-all-area")]
        public async Task<IActionResult> GetAllArea()
        {
            var result = await _areaService.GetAllArea();
            return Ok(result);
        }
        [HttpPost("create-area")]
        public async Task<IActionResult> CreateArea(AreaRequestModel areaRequest)
        {
            var result = await _areaService.CreateArea(areaRequest);
            return Ok(result);
        }

        [HttpDelete]
        public Task DeleteArea(int areaid)
        {
            var result = _areaService.DeleteArea(areaid);
            return result;
        }
        [HttpPut("update-area")]
        public async Task<IActionResult> UpdateArea(UpdateAreaRequestModel areaRequestModel)
        {
            var result = await _areaService.UpdateArea(areaRequestModel);
            return Ok(result);
        }
        [HttpGet("get-area-by-district-id")]
        public async Task<IActionResult> GetAllAreaByDistrictIdReponseModel(int districtid)
        {
            var result = await _areaService.GetAllAreaByDistrictId(districtid);
            return Ok(result);
        }
        [HttpGet("get-single-area-by-area-id")]
        public async Task<IActionResult> GetSingleAreaByAreaId(int areaid)
        {
            var result = await _areaService.GetSingleAreaByAreaId(areaid);
            return Ok(result);
        }
        [HttpGet("get-all-area-by-session-id")]
        public async Task<IActionResult> GetAllAreaBySessionId(int sessionId)
        {
            var result = await _areaService.GetAllAreaBySessionId(sessionId);
            return Ok(result);
        }
        [HttpGet("get-all-area-by-session-id-for-customer")]
        public async Task<IActionResult> GetAllAreaBySessionIdForCustomer(int sessionId)
        {
            var result = await _areaService.GetAllAreaBySessionIdForCustomer(sessionId);
            return Ok(result);
        }

    }
}
