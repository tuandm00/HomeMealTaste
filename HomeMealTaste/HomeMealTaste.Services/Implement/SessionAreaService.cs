using AutoMapper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Services.Implement
{
    public class SessionAreaService : ISessionAreaService
    {
        private readonly HomeMealTasteContext _context;
        private readonly IMapper _mapper;
        private readonly ISessionAreaRepository _sessionareaRepository;

        public SessionAreaService(HomeMealTasteContext context, IMapper mapper, ISessionAreaRepository sessionAreaRepository)
        {
            _context = context;
            _mapper = mapper;
            _sessionareaRepository = sessionAreaRepository;
        }

        public async Task<bool> ChangeStatusSessionArea(int sessionId)
        {
            var getListArea = _context.SessionAreas.Where(x => x.SessionId == sessionId).Select(x => x.AreaId).ToList();
            var result = _context.SessionAreas.Where(x => x.SessionId == sessionId).ToList();
            foreach (var area in getListArea)
            {
                var getListMealSessionId = _context.MealSessions.Where(x => x.SessionId == sessionId && getListArea.Contains(x.AreaId)).Select(x => x.MealSessionId).ToList();
                var getListStatusOfMealSession = _context.MealSessions.Where(x => x.SessionId == sessionId && getListArea.Contains(x.AreaId)).Select(x => x.Status).ToList();
                var getListStatusOrder = _context.Orders.Where(x => getListMealSessionId.Contains((int)x.MealSessionId)).Select(x => x.Status).ToList();

                foreach (var listStatusMealSession in getListStatusOfMealSession)
                {
                    if (listStatusMealSession.Equals("COMPLETED", StringComparison.OrdinalIgnoreCase) || listStatusMealSession.Equals("CANCELLED", StringComparison.OrdinalIgnoreCase))
                    {
                        foreach(var listStatusOrder in getListStatusOrder)
                        {
                            if (listStatusOrder.Equals("COMPLETED", StringComparison.OrdinalIgnoreCase) || listStatusOrder.Equals("CANCELLED", StringComparison.OrdinalIgnoreCase))
                            {
                                foreach(var r in result)
                                {
                                    r.Status = false;
                                }
                            }
                            else
                            {
                                throw new Exception("Some Status Order is Not COMPLETED");
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Some Status Meal Session is Not COMPLETED");
                    }
                }
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> CheckChangeStatusSessionArea(int sessionId)
        {
            try
            {
                bool result = await ChangeStatusSessionArea(sessionId);

                if (result)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }
        public async Task<List<GetAllSessionAreaResponseModel>> GetAllSessionArea()
        {
            var result = _context.SessionAreas.Select(x => new GetAllSessionAreaResponseModel
            {
                SessionAreaId = x.SessionAreaId,
                AreaId = x.AreaId,
                SessionId = x.SessionId,
                Status = x.Status,
            }).ToList();

            var mapped = result.Select(r => _mapper.Map<GetAllSessionAreaResponseModel>(r)).ToList();
            return mapped;
        }


        public async Task<List<GetAllSessionAreaResponseModel>> UpdateStatusSessionArea(UpdateStatusSessionAreaRequestModel request)
        {
            var result = _context.SessionAreas.Where(x => request.SessionAreaIds.Contains(x.SessionAreaId)).ToList();
            if (result == null)
            {
                throw new Exception("Can not find Session Area");
            }
            else
            {
                foreach (var r in result)
                {
                    r.Status = false;
                }

                await _context.SaveChangesAsync();

                var mapped = result.Select(q => _mapper.Map<GetAllSessionAreaResponseModel>(q)).ToList();
                return mapped;
            }

            return null;
        }
    }
}
