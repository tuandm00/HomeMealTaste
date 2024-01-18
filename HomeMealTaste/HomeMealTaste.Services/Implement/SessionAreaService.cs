using AutoMapper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Interface;
using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> ChangeStatusSessionArea(List<int> sessionArea, string status)
        {
            bool check = false;
            var sessionArea1 = sessionArea.FirstOrDefault();
            var session1 = _context.SessionAreas.Where(x => x.SessionAreaId == sessionArea1).Select(x => x.SessionId).FirstOrDefault();
            var SessionItem = _context.Sessions
                                            .Where(x => x.SessionId == session1)
                                            .FirstOrDefault();

            if (status.Equals("FINISHED"))
            {
                if (SessionItem != null && SessionItem.Status.Equals("ONGOING"))
                {
                    foreach (var sessionAreaItem in sessionArea)
                    {
                        var getSessionArea = _context.SessionAreas.Where(x => x.SessionAreaId == sessionAreaItem).FirstOrDefault();
                        var getListMealSession = _context.MealSessions
                            .Where(x => x.SessionId == getSessionArea.SessionId && x.AreaId == getSessionArea.AreaId)
                            .ToList();

                        if (getListMealSession.Count > 0)
                        {


                            foreach (var mealSessionItem in getListMealSession)
                            {
                                var getListOrder = _context.Orders.Where(x => x.MealSessionId == mealSessionItem.MealSessionId).ToList();
                                if (mealSessionItem.Status.Equals("COMPLETED"))
                                {
                                    if (getListOrder.Count > 0)
                                    {
                                        foreach (var orderItem in getListOrder)
                                        {
                                            if (orderItem.Status.Equals("COMPLETED") || orderItem.Status.Equals("CANCELLED") || orderItem.Status.Equals("NOTEAT"))
                                            {
                                                check = true;
                                            }
                                            else
                                            {
                                                check = false;
                                                throw new Exception($"Can not change status to FINISHED because Order {orderItem.OrderId} is not final state");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        check = true;

                                    }
                                }
                                else if (mealSessionItem.Status.Equals("CANCELLED"))
                                {
                                    if (getListOrder.Count > 0)
                                    {
                                        foreach (var orderItem in getListOrder)
                                        {
                                            if (orderItem.Status.Equals("CANCELLED"))
                                            {
                                                check = true;
                                            }
                                            else
                                            {
                                                check = false;
                                                throw new Exception($"Can not change status to FINISHED because Order {orderItem.OrderId} is not final state");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        check = true;
                                    }

                                }
                                else if (mealSessionItem.Status.Equals("REJECTED"))
                                {
                                    check = true;
                                }
                                else
                                {
                                    check = false;
                                    throw new Exception($"Meal Session {mealSessionItem.MealSessionId} is not final state can not change status to FINISHED");
                                }
                            }
                        }
                        else
                        {
                            check = true;
                        }
                        if (check)
                        {
                            getSessionArea.Status = "FINISHED";
                            _context.SessionAreas.Update(getSessionArea);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            throw new Exception("Can not change Status Session Area to FINISHED");
                        }
                    }
                }
                else
                {
                    throw new Exception($"Can not change status to Cancelled because Session {SessionItem.SessionId} is Not ONGOING");
                }
            }
            else if (status.Equals("CANCELLED"))
            {

                if (SessionItem != null && SessionItem.Status.Equals("OPEN"))
                {
                    foreach (var sessionAreaItem in sessionArea)
                    {
                        var getSessionArea = _context.SessionAreas.Where(x => x.SessionAreaId == sessionAreaItem).FirstOrDefault();
                        var getListMealSession = _context.MealSessions
                            .Where(x => x.SessionId == getSessionArea.SessionId && x.AreaId == getSessionArea.AreaId)
                            .ToList();
                        if(getListMealSession.Count == 0)
                        {
                            check = true;
                        }
                        else
                        {
                            foreach (var listMS in getListMealSession)
                            {
                                if (listMS.Status.Equals("REJECTED"))
                                {
                                    check = true;
                                }
                                else if (listMS.Status.Equals("CANCELLED"))
                                {
                                    check = true;
                                }
                                else
                                {
                                    check = false;
                                    throw new Exception("Can not change status to Cancelled because Meal Session must be REJECTED or CANCELLED without Order");
                                }
                            }
                        }
                        
                        if (check)
                        {
                            getSessionArea.Status = "CANCELLED";
                            _context.SessionAreas.Update(getSessionArea);
                            await _context.SaveChangesAsync();

                        }
                        else
                        {
                            throw new Exception("Can not change Status Sessioin Area to CANCELLED");
                        }
                    }

                }
                else
                {
                    throw new Exception("Can not change status to Cancelled because Session is Not Open");
                }
            }
            else
            {
                throw new Exception("There area something wrong");
            }
            await _context.SaveChangesAsync();
            return check;
        }

        public async Task<bool> CheckChangeStatusSessionArea(int sessionId)
        {
            //try
            //{
            //    bool result = await ChangeStatusSessionArea(sessionId);

            //    if (result)
            //    {
            //        return true;
            //    }
            //}
            //catch (Exception ex)
            //{

            //}
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

        public async Task<GetSingleSessionAreaByAreaIdResponseModel> GetSingleSessionAreaBySessionAreaId(int sessionAreaId)
        {
            var result = _context.SessionAreas
                .Include(x => x.Area)
                .ThenInclude(x => x.MealSessions)
                .ThenInclude(x => x.Orders)
                .Where(x => x.SessionAreaId == sessionAreaId)
                .Select(x => new GetSingleSessionAreaByAreaIdResponseModel
                {
                    SessionAreaId = x.SessionAreaId,
                    SessionId = x.SessionId,
                    Status = x.Status,
                    AreaDtoForSessionArea = new AreaDtoForSessionArea
                    {
                        AreaId = x.Area.AreaId,
                        Address = x.Area.Address,
                        AreaName = x.Area.AreaName,
                    },
                    GetAllKitchenByAreaIdResponseModelDto = x.Area.MealSessions.Select(ms => new GetAllKitchenByAreaIdResponseModelDto
                    {
                        KitchenDtoForSessionArea = new KitchenDtoForSessionArea
                        {
                            KitchenId = ms.Kitchen.KitchenId,
                            UserId = ms.Kitchen.UserId,
                            Name = ms.Kitchen.Name,
                            Address = ms.Kitchen.Address,
                            AreaId = ms.Kitchen.AreaId,
                            DistrictId = ms.Kitchen.DistrictId,
                        },
                        SumOfMealSession = _context.MealSessions.Count(mealSession => mealSession.SessionId == x.SessionId && mealSession.AreaId == x.AreaId),
                        SumOfOrder = ms.Orders
                    .Where(order => _context.MealSessions.Any(mealSession => mealSession.SessionId == x.SessionId && mealSession.AreaId == x.AreaId))
                    .Count(),
                    }).GroupBy(dto => new { dto.KitchenDtoForSessionArea.KitchenId, dto.KitchenDtoForSessionArea.UserId, dto.KitchenDtoForSessionArea.Name, dto.KitchenDtoForSessionArea.Address, dto.KitchenDtoForSessionArea.AreaId})
                .Select(group => group.First())
                .ToList(),
                }).FirstOrDefault();

            var mapped = _mapper.Map<GetSingleSessionAreaByAreaIdResponseModel>(result);
            return mapped;
        }

        public async Task<List<GetAllSessionAreaBySessionIdResponseModel>> GetAllSessionAreaBySessionId(int sessionId)
        {
            var result = _context.SessionAreas.Where(x => x.SessionId == sessionId).Select(x => new GetAllSessionAreaBySessionIdResponseModel
            {
                SessionAreaId = x.SessionAreaId,
                SessionId = x.SessionId,
                Status = x.Status,
                AreaDtoForSessionArea = new AreaDtoForSessionArea
                {
                    AreaId = x.Area.AreaId,
                    Address = x.Area.Address,
                    AreaName = x.Area.AreaName,
                }
            }).ToList();
            var mapped = result.Select(r => _mapper.Map<GetAllSessionAreaBySessionIdResponseModel>(r)).ToList();
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
                if (result.Equals("OPEN") && request.Status.Equals("FINISHED", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var r in result)
                    {
                        r.Status = "FINISHED";
                    }
                }
                else if (result.Equals("OPEN") && request.Status.Equals("CANCELLED", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var r in result)
                    {
                        r.Status = "CANCELLED";
                    }
                }

                await _context.SaveChangesAsync();

                var mapped = result.Select(q => _mapper.Map<GetAllSessionAreaResponseModel>(q)).ToList();
                return mapped;
            }

            return null;
        }
    }
}
