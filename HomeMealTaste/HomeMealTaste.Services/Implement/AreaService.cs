using AutoMapper;
using HomeMealTaste.Data.Helper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Helper;
using HomeMealTaste.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Services.Implement
{
    public class AreaService : IAreaService
    {
        private readonly IAreaRepository _areaRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AreaService> _logger;
        private readonly HomeMealTasteContext _context;

        public AreaService(IAreaRepository areaRepository, IMapper mapper, HomeMealTasteContext context, ILogger<AreaService> logger)
        {
            _areaRepository = areaRepository;
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }

        public async Task<AreaResponseModel> CreateArea(AreaRequestModel areaRequest)
        {
            var mapped = new AreaResponseModel();
            try
            {
                var entity = _mapper.Map<Area>(areaRequest);
                entity.Address = areaRequest.Address;
                entity.DistrictId = areaRequest.DistrictId;
                entity.AreaName = areaRequest.AreaName;

                _context.Areas.Add(entity);
                await _context.SaveChangesAsync();

                mapped = _mapper.Map<AreaResponseModel>(entity);
                mapped.Message = "Success";
                //if (mapped != null && areaRequest.SessionIds != null)
                //{
                //    foreach (var i in areaRequest.SessionIds)
                //    {
                //        var addToSessionArea = new SessionArea
                //        {
                //            AreaId = mapped.AreaId,
                //            SessionId = i,
                //        };
                //        _context.SessionAreas.Add(addToSessionArea);
                //    }

                //    await _context.SaveChangesAsync();
                //    mapped.Message = "Success";
                //}

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in Create Area: {Message}", ex.Message);
                mapped.Message = "Error: " + ex.Message;
            }
            return mapped;
        }

        public Task DeleteArea(int areaid)
        {
            var result = _areaRepository.Delete(areaid);
            return result;
        }

        public async Task<List<AreaResponseModel>> GetAllArea()
        {
            var result = _context.Areas.Select(x => new AreaResponseModel
            {
                AreaId = x.AreaId,
                Address = x.Address,
                AreaName = x.AreaName,
                DistrictDtoAreaResponseModel = new DistrictDtoAreaResponseModel
                {
                    DistrictId = x.District.DistrictId,
                    DistrictName = x.District.DistrictName,
                }
            }).ToList();

            var mapped = result.Select(x => _mapper.Map<AreaResponseModel>(x)).ToList();
            return mapped;
        }

        public async Task<List<GetAllAreaByDistrictIdReponseModel>> GetAllAreaByDistrictId(int districtid)
        {
            var result = _context.Areas.Where(x => x.DistrictId == districtid).Select(x => new GetAllAreaByDistrictIdReponseModel
            {
                AreaId = x.AreaId,
                Address = x.Address,
                AreaName = x.AreaName,
                DistrictDtoResponse = new DistrictDtoResponse
                {
                    DistrictId = x.District.DistrictId,
                    DistrictName = x.District.DistrictName,
                }
            }).ToList();

            var mapped = result.Select(x => _mapper.Map<GetAllAreaByDistrictIdReponseModel>(x)).ToList();
            return mapped;
        }

        public async Task<GetAllAreaResponse> GetAllAreaBySessionId(int sessionId)
        {
            var getListArea = _context.SessionAreas
                .Include(s => s.Area)
                .Include(s => s.Session)
                .ThenInclude(s => s.MealSessions)
                .ThenInclude(s => s.Orders)
                .Where(s => s.SessionId == sessionId)
                .Select(s => new GetAllAreaBySessionIdResponseModel
                {
                    AreaId = s.Area.AreaId,
                    Address = s.Area.Address,
                    AreaName = s.Area.AreaName,
                    DistrictId = s.Area.DistrictId,
                    TotalChefs = s.Session.MealSessions.Select(ms => ms.KitchenId).Distinct().Count(),
                    TotalMealSessions = s.Session.MealSessions.Count(),
                    TotalOrders = s.Session.MealSessions.SelectMany(ms => ms.Orders).Count(),
                }).ToList();

            var totalPriceOrders = _context.SessionAreas
                .Where(s => s.SessionId == sessionId)
                .SelectMany(s => s.Session.MealSessions.SelectMany(ms => ms.Orders))
                .Select(x => x.TotalPrice)
                .Distinct()
                .Sum();
            var totalOrdersWithStatusPaid = _context.SessionAreas
    .Where(s => s.SessionId == sessionId)
    .SelectMany(s => s.Session.MealSessions.SelectMany(ms => ms.Orders))
    .Count(x => x.Status == "PAID");

            var totalOrdersWithStatusAccepted = _context.SessionAreas
    .Where(s => s.SessionId == sessionId)
    .SelectMany(s => s.Session.MealSessions.SelectMany(ms => ms.Orders))
    .Count(x => x.Status == "ACCEPTED");

            var totalOrdersWithStatusCompleted = _context.SessionAreas
    .Where(s => s.SessionId == sessionId)
    .SelectMany(s => s.Session.MealSessions.SelectMany(ms => ms.Orders))
    .Count(x => x.Status == "COMPLETED");

            var totalOrdersWithStatusCancelled = _context.SessionAreas
    .Where(s => s.SessionId == sessionId)
    .SelectMany(s => s.Session.MealSessions.SelectMany(ms => ms.Orders))
    .Count(x => x.Status == "CANCELLED");

            var totalOrdersWithStatusNotEat = _context.SessionAreas
    .Where(s => s.SessionId == sessionId)
    .SelectMany(s => s.Session.MealSessions.SelectMany(ms => ms.Orders))
    .Count(x => x.Status == "NOTEAT");

            var sumTotalMealSessions = getListArea.Sum(a => a.TotalMealSessions) ?? 0;
            var sumTotalOrders = getListArea.Sum(a => a.TotalOrders) ?? 0;
            var sumTotalChefs = getListArea.Sum(a => a.TotalChefs) ?? 0;

            var response = new GetAllAreaResponse
            {
                AreaList = _mapper.Map<List<GetAllAreaBySessionIdResponseModel>>(getListArea),
                TotalPriceOrders = totalPriceOrders,
                TotalOrdersWithStatusPaid = totalOrdersWithStatusPaid,
                TotalOrdersWithStatusAccepted = totalOrdersWithStatusAccepted,
                TotalOrdersWithStatusCancelled = totalOrdersWithStatusCancelled,
                TotalOrdersWithStatusCompleted = totalOrdersWithStatusCompleted,
                TotalOrdersWithStatusNotEat = totalOrdersWithStatusNotEat,
                SumTotalChefs = sumTotalChefs,
                SumTotalMealSessions = sumTotalMealSessions,
                SumTotalOrders = sumTotalOrders,
            };

            return response;
        }

        public async Task<List<GetAllAreaBySessionIdForCustomerResponseModel>> GetAllAreaBySessionIdForCustomer(int sessionId)
        {
            var result = _context.SessionAreas.Include(x => x.Area).Where(x => x.SessionId == sessionId).Select(x => new GetAllAreaBySessionIdForCustomerResponseModel
            {
                AreaId = x.Area.AreaId,
                Address = x.Area.Address,
                AreaName = x.Area.AreaName,
            }).ToList();

            var mapped = result.Select(r => _mapper.Map<GetAllAreaBySessionIdForCustomerResponseModel>(r)).ToList();
            return mapped;
        }

        public async Task<GetSingleAreaByAreaIdResponseModel> GetSingleAreaByAreaId(int areaid)
        {
            var result = _context.Areas.Where(a => a.AreaId == areaid).Select(a => new GetSingleAreaByAreaIdResponseModel
            {
                AreaName = a.AreaName,
                AreaId = a.AreaId,
                Address = a.Address,
                DistrictDtoGetSingleAreaByAreaId = new DistrictDtoGetSingleAreaByAreaId
                {
                    DistrictId = a.District.DistrictId,
                    DistrictName = a.District.DistrictName,
                }
            }).FirstOrDefault();

            var mapped = _mapper.Map<GetSingleAreaByAreaIdResponseModel>(result);
            return mapped;
        }

        public Task<UpdateAreaResponseModel> UpdateArea(UpdateAreaRequestModel areaRequestModel)
        {
            var result = _context.Areas.Where(x => x.AreaId == areaRequestModel.AreaId).FirstOrDefault();
            if (result != null)
            {
                result.AreaId = areaRequestModel.AreaId;
                result.Address = areaRequestModel.Address;
                result.DistrictId = areaRequestModel.DistrictId;
                result.AreaName = areaRequestModel.AreaName;

                _context.SaveChanges();
            }

            var mapped = _mapper.Map<UpdateAreaResponseModel>(result);
            return Task.FromResult(mapped);
        }
    }
}
