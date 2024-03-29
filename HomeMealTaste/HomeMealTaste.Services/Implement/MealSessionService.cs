﻿using AutoMapper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace HomeMealTaste.Services.Implement
{
    public class MealSessionService : IMealSessionService
    {
        private readonly IMealSessionRepository _mealSessionRepository;
        private readonly IMapper _mapper;
        private readonly IDishRepository _dishRepository;
        private readonly IMealRepository _mealRepository;
        private readonly IMealDishRepository _mealDishRepository;
        private readonly INotificationService _notificationService;
        private readonly HomeMealTasteContext _context;

        public MealSessionService(IMealSessionRepository mealSessionRepository, IMapper mapper,
            IDishRepository dishRepository, IMealRepository mealRepository, IMealDishRepository mealDishRepository, HomeMealTasteContext context, INotificationService notificationService)
        {
            _mealSessionRepository = mealSessionRepository;
            _mapper = mapper;
            _dishRepository = dishRepository;
            _mealRepository = mealRepository;
            _mealDishRepository = mealDishRepository;
            _context = context;
            _notificationService = notificationService;
        }
        public static DateTime TranferDateTimeByTimeZone(DateTime dateTime, string timezoneArea)
        {

            ReadOnlyCollection<TimeZoneInfo> collection = TimeZoneInfo.GetSystemTimeZones();
            var timeZone = collection.ToList().Where(x => x.DisplayName.ToLower().Contains(timezoneArea)).First();

            var timeZoneLocal = TimeZoneInfo.Local;

            var utcDateTime = TimeZoneInfo.ConvertTime(dateTime, timeZoneLocal, timeZone);

            return utcDateTime;
        }

        public static DateTime GetDateTimeTimeZoneVietNam()
        {

            return TranferDateTimeByTimeZone(DateTime.Now, "hanoi");
        }
        public static DateTime? StringToDateTimeVN(string dateStr)
        {

            var isValid = System.DateTime.TryParseExact(
                                dateStr,
                                "d'/'M'/'yyyy",
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None,
                                out var date
                            );
            return isValid ? date : null;
        }
        public async Task<List<MealSessionResponseModel>> CreateMealSession(MealSessionRequestModel mealSessionRequest)
        {
            var mappedResults = new List<MealSessionResponseModel>();
            var datenow = GetDateTimeTimeZoneVietNam();
            if (mealSessionRequest.SessionIds != null && mealSessionRequest.SessionIds.Any())
            {
                var mealSessions = _context.MealSessions
                    .Where(x => x.CreateDate.Value.Date == datenow.Date && x.KitchenId == mealSessionRequest.KitchenId).ToList();

                foreach (var miu in mealSessions)
                {
                    foreach (var session in mealSessionRequest.SessionIds)
                    {
                        if (session == miu.SessionId && miu.Status.Equals("PROCESSING"))
                        {
                            throw new Exception("Can not Create Meal Session because Meal is Existed in this Session");
                        }
                    }
                }
                if (mealSessions.Count > 0)
                {

                    foreach (var mealSession in mealSessionRequest.SessionIds)
                    {
                        //var statusExist = _context.MealSessions
                        //    .Where(x => x.CreateDate.Value.Date == datenow.Date && x.KitchenId == mealSessionRequest.KitchenId && x.SessionId == session)
                        //    .Select(x => x.Status)
                        //    .ToList();

                        //if (session == mealsession.SessionId && mealsession.Status.Equals("PROCESSING"))
                        //{
                        //    throw new Exception("");
                        //}

                        var entityForMealSessionId = _mapper.Map<MealSession>(mealSessionRequest);
                        var getKitchenArea = _context.Kitchens.Where(x => x.KitchenId == mealSessionRequest.KitchenId).Select(x => x.AreaId).FirstOrDefault();
                        entityForMealSessionId.MealId = mealSessionRequest.MealId;
                        entityForMealSessionId.SessionId = mealSession;
                        entityForMealSessionId.Quantity = mealSessionRequest.Quantity;
                        entityForMealSessionId.RemainQuantity = entityForMealSessionId.Quantity;
                        entityForMealSessionId.KitchenId = mealSessionRequest.KitchenId;
                        entityForMealSessionId.Status = "PROCESSING";
                        entityForMealSessionId.CreateDate = GetDateTimeTimeZoneVietNam();
                        entityForMealSessionId.AreaId = getKitchenArea;

                        var resultForSessionId = await _mealSessionRepository.Create(entityForMealSessionId, true);

                        await _context.Entry(resultForSessionId).Reference(m => m.Meal).LoadAsync();
                        await _context.Entry(resultForSessionId).Reference(m => m.Session).LoadAsync();
                        await _context.Entry(resultForSessionId.Meal).Reference(s => s.Kitchen).LoadAsync();
                        await _context.Entry(resultForSessionId.Kitchen).Reference(a => a.Area).LoadAsync();

                        var mappedForSessionId = _mapper.Map<MealSessionResponseModel>(resultForSessionId);

                        mappedResults.Add(mappedForSessionId);

                        //else if(sessio)
                        //{
                        //    var entityForSessionId = _mapper.Map<MealSession>(mealSessionRequest);

                        //    entityForSessionId.MealId = mealSessionRequest.MealId;
                        //    entityForSessionId.SessionId = session;
                        //    entityForSessionId.Quantity = mealSessionRequest.Quantity;
                        //    entityForSessionId.RemainQuantity = entityForSessionId.Quantity;
                        //    entityForSessionId.KitchenId = mealSessionRequest.KitchenId;
                        //    entityForSessionId.Status = "PROCESSING";
                        //    entityForSessionId.CreateDate = GetDateTimeTimeZoneVietNam();

                        //    var resultForSessionId = await _mealSessionRepository.Create(entityForSessionId, true);

                        //    await _context.Entry(resultForSessionId).Reference(m => m.Meal).LoadAsync();
                        //    await _context.Entry(resultForSessionId).Reference(m => m.Session).LoadAsync();
                        //    await _context.Entry(resultForSessionId.Meal).Reference(s => s.Kitchen).LoadAsync();
                        //    await _context.Entry(resultForSessionId.Kitchen).Reference(a => a.Area).LoadAsync();

                        //    var mappedForSessionId = _mapper.Map<MealSessionResponseModel>(resultForSessionId);

                        //    mappedResults.Add(mappedForSessionId);
                        //}
                    }

                }
                else
                {
                    foreach (var mealSession in mealSessionRequest.SessionIds)
                    {
                        var entityForMealSessionId = _mapper.Map<MealSession>(mealSessionRequest);
                        var getKitchenArea = _context.Kitchens.Where(x => x.KitchenId == mealSessionRequest.KitchenId).Select(x => x.AreaId).FirstOrDefault();

                        entityForMealSessionId.MealId = mealSessionRequest.MealId;
                        entityForMealSessionId.SessionId = mealSession;
                        entityForMealSessionId.Quantity = mealSessionRequest.Quantity;
                        entityForMealSessionId.RemainQuantity = entityForMealSessionId.Quantity;
                        entityForMealSessionId.KitchenId = mealSessionRequest.KitchenId;
                        entityForMealSessionId.Status = "PROCESSING";
                        entityForMealSessionId.CreateDate = GetDateTimeTimeZoneVietNam();
                        entityForMealSessionId.AreaId = getKitchenArea;

                        var resultForSessionId = await _mealSessionRepository.Create(entityForMealSessionId, true);

                        await _context.Entry(resultForSessionId).Reference(m => m.Meal).LoadAsync();
                        await _context.Entry(resultForSessionId).Reference(m => m.Session).LoadAsync();
                        await _context.Entry(resultForSessionId.Meal).Reference(s => s.Kitchen).LoadAsync();
                        await _context.Entry(resultForSessionId.Kitchen).Reference(a => a.Area).LoadAsync();

                        var mappedForSessionId = _mapper.Map<MealSessionResponseModel>(resultForSessionId);

                        mappedResults.Add(mappedForSessionId);
                    }

                }

            }
            return mappedResults;
        }

        public async Task<List<MealSessionResponseModel>> GetAllMealSession()
        {
            var result = _context.MealSessions
                .Include(x => x.Meal)
                .Include(x => x.Session).ThenInclude(x => x.SessionAreas).ThenInclude(x => x.Area)
                .Include(x => x.Kitchen).Select(x => new MealSessionResponseModel
                {
                    MealSessionId = x.MealSessionId,
                    CreateDate = ((DateTime)x.CreateDate).ToString("dd-MM-yyyy"),
                    Price = (decimal?)x.Price,
                    Quantity = x.Quantity,
                    RemainQuantity = x.RemainQuantity,
                    Status = x.Status,
                    AreaDtoForMealSession = new AreaDtoForMealSession
                    {
                        AreaId = x.Area.AreaId,
                        Address = x.Area.Address,
                        AreaName = x.Area.AreaName,
                    },
                    KitchenDtoForMealSession = new KitchenDtoForMealSession
                    {
                        KitchenId = x.Meal.Kitchen.KitchenId,
                        UserId = x.Meal.Kitchen.KitchenId,
                        Name = x.Meal.Kitchen.Name,
                        Address = x.Meal.Kitchen.Address,
                        AreaDtoForMealSession = new AreaDtoForMealSession
                        {
                            AreaId = x.Meal.Kitchen.Area.AreaId,
                            Address = x.Meal.Kitchen.Area.Address,
                            AreaName = x.Meal.Kitchen.Area.AreaName,
                        },
                    },
                    SessionDtoForMealSession = new SessionDtoForMealSession
                    {
                        SessionId = x.Session.SessionId,
                        CreateDate = ((DateTime)x.Session.CreateDate).ToString("dd-MM-yyyy"),
                        StartTime = ((DateTime)x.Session.StartTime).ToString("HH:mm"),
                        EndTime = ((DateTime)x.Session.EndTime).ToString("HH:mm"),
                        EndDate = ((DateTime)x.Session.EndDate).ToString("dd-MM-yyyy"),
                        UserId = x.Session.UserId,
                        Status = x.Session.Status,
                        SessionType = x.Session.SessionType,
                        SessionName = x.Session.SessionName,
                    },
                    MealDtoForMealSession = new MealDtoForMealSession
                    {
                        MealId = x.Meal.MealId,
                        Name = x.Meal.Name,
                        Image = x.Meal.Image,
                        KitchenId = x.KitchenId,
                        CreateDate = ((DateTime)x.Meal.CreateDate).ToString("dd-MM-yyyy"),
                        Description = x.Meal.Description,
                    },
                })
                .ToList();

            var mapped = result.Select(r => _mapper.Map<MealSessionResponseModel>(r)).ToList();
            return mapped;
            //var mapped = result.Select(mealsession =>
            //{
            //    var response = _mapper.Map<MealSessionResponseModel>(mealsession);
            //    response.MealSessionId = mealsession.MealSessionId;
            //    response.MealDtoForMealSession = new MealDtoForMealSession
            //    {
            //MealId = mealsession.Meal.MealId,
            //        Name = mealsession.Meal.Name,
            //        Image = mealsession.Meal.Image,
            //        KitchenId = mealsession.KitchenId,
            //        CreateDate = ((DateTime)mealsession.Meal.CreateDate).ToString("dd-MM-yyyy"),
            //        Description = mealsession.Meal?.Description,
            //    };
            //    response.SessionDtoForMealSession = new SessionDtoForMealSession
            //    {
            //        SessionId = mealsession.Session.SessionId,
            //        CreateDate = ((DateTime)mealsession.Session.CreateDate).ToString("dd-MM-yyyy"),
            //        StartTime = ((DateTime)mealsession.Session.StartTime).ToString("HH:mm"),
            //        EndTime = ((DateTime)mealsession.Session.EndTime).ToString("HH:mm"),
            //        EndDate = ((DateTime)mealsession.Session.EndDate).ToString("dd-MM-yyyy"),
            //        UserId = mealsession.Session?.UserId,
            //        Status = mealsession.Session.Status,
            //        SessionType = mealsession.Session.SessionType,
            //        SessionName = mealsession?.Session.SessionName,
            //        AreaDtoForMealSession = new AreaDtoForMealSession
            //        {
            //AreaId = mealsession.Session.Area.AreaId,
            //            Address = mealsession.Session.Area.Address,
            //            AreaName = mealsession.Session.Area.AreaName,
            //        },
            //    };
            //    response.KitchenDtoForMealSession = new KitchenDtoForMealSession
            //    {
            //KitchenId = mealsession.Meal.Kitchen.KitchenId,
            //        UserId = mealsession.Meal.Kitchen.KitchenId,
            //        Name = mealsession.Meal.Kitchen?.Name,
            //        Address = mealsession.Meal.Kitchen.Address,
            //    };
            //    response.Price = (decimal?)mealsession.Price;
            //    response.Quantity = mealsession.Quantity;
            //    response.RemainQuantity = mealsession.RemainQuantity;
            //    response.Status = mealsession.Status;
            //    response.CreateDate = ((DateTime)mealsession.CreateDate).ToString("dd-MM-yyyy");

            //    return response;
            //}).ToList();

            //return mapped;
        }

        public async Task<List<MealSessionResponseModel>> GetAllMealSessionByStatus(string status)
        {
            var lowerStatus = status.ToLower();
            var result = _context.MealSessions
                .Include(x => x.Meal)
                .Include(x => x.Session)
                .Include(x => x.Kitchen)
                .Where(x => x.Status.ToLower() == lowerStatus).Select(x => new MealSessionResponseModel
                {
                    MealSessionId = x.MealSessionId,
                    CreateDate = ((DateTime)x.CreateDate).ToString("dd-MM-yyyy"),
                    Price = (decimal?)x.Price,
                    Quantity = x.Quantity,
                    RemainQuantity = x.RemainQuantity,
                    Status = x.Status,
                    KitchenDtoForMealSession = new KitchenDtoForMealSession
                    {
                        KitchenId = x.Meal.Kitchen.KitchenId,
                        UserId = x.Meal.Kitchen.KitchenId,
                        Name = x.Meal.Kitchen.Name,
                        Address = x.Meal.Kitchen.Address,
                        AreaDtoForMealSession = new AreaDtoForMealSession
                        {
                            AreaId = x.Meal.Kitchen.Area.AreaId,
                            AreaName = x.Meal.Kitchen.Area.AreaName,
                            Address = x.Meal.Kitchen.Area.Address,
                        }
                    },
                    SessionDtoForMealSession = new SessionDtoForMealSession
                    {
                        SessionId = x.Session.SessionId,
                        CreateDate = ((DateTime)x.Session.CreateDate).ToString("dd-MM-yyyy"),
                        StartTime = ((DateTime)x.Session.StartTime).ToString("HH:mm"),
                        EndTime = ((DateTime)x.Session.EndTime).ToString("HH:mm"),
                        EndDate = ((DateTime)x.Session.EndDate).ToString("dd-MM-yyyy"),
                        UserId = x.Session.UserId,
                        Status = x.Session.Status,
                        SessionType = x.Session.SessionType,
                        SessionName = x.Session.SessionName,
                    },
                    MealDtoForMealSession = new MealDtoForMealSession
                    {
                        MealId = x.Meal.MealId,
                        Name = x.Meal.Name,
                        Image = x.Meal.Image,
                        KitchenId = x.KitchenId,
                        CreateDate = ((DateTime)x.Meal.CreateDate).ToString("dd-MM-yyyy"),
                        Description = x.Meal.Description,
                    },
                })
                .ToList();
            var mapped = result.Select(r => _mapper.Map<MealSessionResponseModel>(r)).ToList();
            return mapped;
        }

        public async Task<GetSingleMealSessionByIdResponseModel> GetSingleMealSessionById(int mealsessionid)
        {
            var orders = _context.Orders.Where(x => x.MealSessionId == mealsessionid).ToList();
            bool check;
            if (orders.Count > 0)
            {
                check = true;
            }
            else
            {
                check = false;
            }
            var result = _context.MealSessions
    .Include(x => x.Meal).ThenInclude(x => x.MealDishes).ThenInclude(x => x.Dish)
    .Include(x => x.Session).ThenInclude(x => x.SessionAreas).ThenInclude(x => x.Area).ThenInclude(x => x.District)
    .Include(x => x.Kitchen)
    .Where(x => x.MealSessionId == mealsessionid)
    .Select(gr => new GetSingleMealSessionByIdResponseModel
    {
        MealSessionId = gr.MealSessionId,
        Price = (decimal?)gr.Price,
        Quantity = gr.Quantity,
        RemainQuantity = gr.RemainQuantity,
        Status = gr.Status,
        CreateDate = ((DateTime)gr.CreateDate).ToString("dd-MM-yyyy"),
        checkOrderExisted = check,
        AreaDtoForMealSessions = new AreaDtoForMealSessions
        {
            AreaId = gr.Area.AreaId,
            Address = gr.Area.Address,
            AreaName = gr.Area.AreaName,
        },
        MealDtoForMealSessions = new MealDtoForMealSessions
        {
            MealId = gr.Meal.MealId,
            Name = gr.Meal.Name,
            Image = gr.Meal.Image,
            KitchenId = gr.Meal.KitchenId,
            CreateDate = ((DateTime)gr.Meal.CreateDate).ToString("dd-MM-yyyy"),
            Description = gr.Meal.Description,
            DishesDtoMealSession = gr.Meal.MealDishes
                .Select(md => new DishesDtoMealSession
                {
                    DishId = md.Dish.DishId,
                    Name = md.Dish.Name,
                    Image = md.Dish.Image,
                    DishTypeDtoMealSession = new DishTypeDtoMealSession
                    {
                        DishTypeId = md.Dish.DishType.DishTypeId,
                        Description = md.Dish.DishType.Description,
                        Name = md.Dish.DishType.Name,
                    },
                    KitchenId = md.Dish.KitchenId
                })
                .ToList(),
        },
        SessionDtoForMealSessions = new SessionDtoForMealSessions
        {
            SessionId = gr.Session.SessionId,
            CreateDate = ((DateTime)gr.Session.CreateDate).ToString("dd-MM-yyyy"),
            StartTime = ((DateTime)gr.Session.StartTime).ToString("HH:mm"),
            EndTime = ((DateTime)gr.Session.EndTime).ToString("HH:mm"),
            EndDate = ((DateTime)gr.Session.EndDate).ToString("dd-MM-yyyy"),
            UserId = gr.Session.UserId,
            Status = gr.Session.Status,
            SessionType = gr.Session.SessionType,
            SessionName = gr.Session.SessionName,
        },
        KitchenDtoForMealSessions = new KitchenDtoForMealSessions
        {
            KitchenId = gr.Meal.Kitchen.KitchenId,
            UserId = gr.Meal.Kitchen.KitchenId,
            Name = gr.Meal.Kitchen.Name,
            Address = gr.Meal.Kitchen.Address,
        }
    })
    .FirstOrDefault();

            var mapped = _mapper.Map<GetSingleMealSessionByIdResponseModel>(result);
            return mapped;
        }

        public async Task UpdateStatusMeallSession(UpdateStatusMeallSessionRequestModel request)
        {
            if (request.status.Equals("APPROVED" ,StringComparison.OrdinalIgnoreCase))
            {
                bool check = false;
                foreach (var mealSesssionIdItem in request.MealSessionIds)
                {
                    var mealSessionItem = _context.MealSessions.Where(x => x.MealSessionId == mealSesssionIdItem).FirstOrDefault();
                    var sessionItem = _context.Sessions
                   .Where(x => x.SessionId == mealSessionItem.SessionId)
                   .FirstOrDefault();
                    if (mealSessionItem.Status.Equals("PROCESSING", StringComparison.OrdinalIgnoreCase) && sessionItem.Status.Equals("OPEN"))
                    {
                        check = true;
                    }
                    else
                    {
                        check = false;
                        throw new Exception($"Can not change status to APPROVED because Meal Session {mealSessionItem.MealSessionId} have Status is{mealSessionItem.Status} and Session {sessionItem} have Status is{sessionItem.Status}");
                    }
                    if (check)
                    {
                        mealSessionItem.Status = "APPROVED";
                        _context.MealSessions.Update(mealSessionItem);
                        await _notificationService.SendNotificationForChefWhenMealSessionApproved(mealSessionItem.MealSessionId);
                    }
                }
            }
            else if (request.status.Equals("REJECTED", StringComparison.OrdinalIgnoreCase))
            {
                bool check = false;
                foreach (var mealSesssionIdItem in request.MealSessionIds)
                {
                    var mealSessionItem = _context.MealSessions.Where(x => x.MealSessionId == mealSesssionIdItem).FirstOrDefault();
                    var sessionItem = _context.Sessions
                   .Where(x => x.SessionId == mealSessionItem.SessionId)
                   .FirstOrDefault();
                    if (mealSessionItem.Status.Equals("PROCESSING", StringComparison.OrdinalIgnoreCase) && sessionItem.Status.Equals("OPEN"))
                    {
                        check = true;
                    }
                    else
                    {
                        check = false;
                        throw new Exception($"Can not change status to REJECTED because Meal Session {mealSessionItem.MealSessionId} have Status is{mealSessionItem.Status} and Session {sessionItem} have Status is{sessionItem.Status}");
                    }
                    if (check)
                    {
                        mealSessionItem.Status = "REJECTED";
                        _context.MealSessions.Update(mealSessionItem);
                        await _notificationService.SendNotificationForChefWhenMealSessionRejected(mealSessionItem.MealSessionId);
                    }
                }
            }
            else if (request.status.Equals("CANCELLED", StringComparison.OrdinalIgnoreCase))
            {
                bool check = false;
                foreach (var mealSesssionIdItem in request.MealSessionIds)
                {
                    var mealSessionItem = _context.MealSessions.Where(x => x.MealSessionId == mealSesssionIdItem).FirstOrDefault();
                    var sessionItem = _context.Sessions
                   .Where(x => x.SessionId == mealSessionItem.SessionId)
                   .FirstOrDefault();
                    var orderList = _context.Orders.Where(x => x.MealSessionId == mealSessionItem.MealSessionId).ToList();
                    if (orderList.Count == 0)
                    {
                        if (sessionItem.Status.Equals("BOOKING") || sessionItem.Status.Equals("OPEN"))
                        {
                            check = true;
                        }
                        else
                        {
                            check = false;
                            throw new Exception($"Can not change status to CANCELLED because Meal Session {mealSessionItem.MealSessionId} have Status is{mealSessionItem.Status} and Session {sessionItem} have Status is{sessionItem.Status}");
                        }
                    }
                    else
                    {
                        bool allOrdersCancelled = orderList.All(order => order.Status == "CANCELLED");

                        if (allOrdersCancelled)
                        {
                            check = true;
                        }
                        else
                        {
                            check = false;
                            throw new Exception("Can not change status to CANCELLED Please Check Status Order to final State ");
                        }
                      
                    }

                    if (check)
                    {
                        mealSessionItem.Status = "CANCELLED";
                        _context.MealSessions.Update(mealSessionItem);
                        await _notificationService.SendNotificationForChefWhenMealSessionCancelled(mealSessionItem.MealSessionId);
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        public Task<List<MealSessionResponseModel>> GetAllMeallSessionBySessionIdINDAY(int sessionid)
        {
            var datenow = GetDateTimeTimeZoneVietNam();
            var result = _context.MealSessions
                .Include(x => x.Meal)
                .Include(x => x.Session).ThenInclude(x => x.SessionAreas).ThenInclude(x => x.Area).ThenInclude(x => x.District)
                .Include(x => x.Kitchen)
                .Where(x => x.SessionId == sessionid && x.CreateDate == datenow)
                .ToList();
            var mapped = result.Select(mealsession =>
            {
                var response = _mapper.Map<MealSessionResponseModel>(mealsession);
                response.MealSessionId = mealsession.MealSessionId;
                response.MealDtoForMealSession = new MealDtoForMealSession
                {
                    MealId = mealsession.Meal.MealId,
                    Name = mealsession.Meal.Name,
                    Image = mealsession.Meal.Image,
                    KitchenId = mealsession.KitchenId,
                    CreateDate = ((DateTime)mealsession.Meal.CreateDate).ToString("dd-MM-yyyy"),
                    Description = mealsession.Meal?.Description,
                };
                response.SessionDtoForMealSession = new SessionDtoForMealSession
                {
                    SessionId = mealsession.Session.SessionId,
                    CreateDate = ((DateTime)mealsession.Session.CreateDate).ToString("dd-MM-yyyy"),
                    StartTime = ((DateTime)mealsession.Session.StartTime).ToString("HH:mm"),
                    EndTime = ((DateTime)mealsession.Session.EndTime).ToString("HH:mm"),
                    EndDate = ((DateTime)mealsession.Session.EndDate).ToString("dd-MM-yyyy"),
                    UserId = mealsession.Session?.UserId,
                    Status = mealsession.Session.Status,
                    SessionType = mealsession.Session.SessionType,
                    SessionName = mealsession.Session.SessionName,


                };
                response.KitchenDtoForMealSession = new KitchenDtoForMealSession
                {
                    KitchenId = mealsession.Meal.Kitchen.KitchenId,
                    UserId = mealsession.Meal.Kitchen.KitchenId,
                    Name = mealsession.Meal.Kitchen?.Name,
                    Address = mealsession.Meal.Kitchen.Address,
                    AreaDtoForMealSession = new AreaDtoForMealSession
                    {
                        AreaId = mealsession.Meal.Kitchen.Area.AreaId,
                        Address = mealsession.Meal.Kitchen.Area.Address,
                        AreaName = mealsession.Meal.Kitchen.Area.AreaName,
                    }
                };
                response.Price = (decimal?)mealsession.Price;
                response.Quantity = mealsession.Quantity;
                response.RemainQuantity = mealsession.RemainQuantity;
                response.Status = mealsession.Status;
                response.CreateDate = ((DateTime)mealsession.CreateDate).ToString("dd-MM-yyyy");

                return response;
            }).ToList();

            return Task.FromResult(mapped);
        }

        public Task<List<MealSessionResponseModel>> GetAllMeallSessionBySessionId(int sessionid)
        {
            var datenow = GetDateTimeTimeZoneVietNam();
            var result = _context.MealSessions
                .Include(x => x.Meal)
                .Include(x => x.Session).ThenInclude(x => x.SessionAreas).ThenInclude(x => x.Area).ThenInclude(x => x.District)
                .Include(x => x.Kitchen)
                .Where(x => x.SessionId == sessionid).Select(x => new MealSessionResponseModel
                {
                    MealSessionId = x.MealSessionId,
                    CreateDate = ((DateTime)x.CreateDate).ToString("dd-MM-yyyy"),
                    Price = (decimal?)x.Price,
                    Quantity = x.Quantity,
                    RemainQuantity = x.RemainQuantity,
                    Status = x.Status,
                    AreaDtoForMealSession = new AreaDtoForMealSession
                    {
                        AreaId = x.Area.AreaId,
                        Address = x.Area.Address,
                        AreaName = x.Area.AreaName,
                    },
                    KitchenDtoForMealSession = new KitchenDtoForMealSession
                    {
                        KitchenId = x.Meal.Kitchen.KitchenId,
                        UserId = x.Meal.Kitchen.KitchenId,
                        Name = x.Meal.Kitchen.Name,
                        Address = x.Meal.Kitchen.Address,
                        AreaDtoForMealSession = new AreaDtoForMealSession
                        {
                            AreaId = x.Meal.Kitchen.Area.AreaId,
                            Address = x.Meal.Kitchen.Area.Address,
                            AreaName = x.Meal.Kitchen.Area.AreaName,
                        }
                    },
                    SessionDtoForMealSession = new SessionDtoForMealSession
                    {
                        SessionId = x.Session.SessionId,
                        CreateDate = ((DateTime)x.Session.CreateDate).ToString("dd-MM-yyyy"),
                        StartTime = ((DateTime)x.Session.StartTime).ToString("HH:mm"),
                        EndTime = ((DateTime)x.Session.EndTime).ToString("HH:mm"),
                        EndDate = ((DateTime)x.Session.EndDate).ToString("dd-MM-yyyy"),
                        UserId = x.Session.UserId,
                        Status = x.Session.Status,
                        SessionType = x.Session.SessionType,
                        SessionName = x.Session.SessionName,
                    },
                    MealDtoForMealSession = new MealDtoForMealSession
                    {
                        MealId = x.Meal.MealId,
                        Name = x.Meal.Name,
                        Image = x.Meal.Image,
                        KitchenId = x.KitchenId,
                        CreateDate = ((DateTime)x.Meal.CreateDate).ToString("dd-MM-yyyy"),
                        Description = x.Meal.Description,
                    },
                })
                .ToList();

            var mapped = result.Select(r => _mapper.Map<MealSessionResponseModel>(r)).ToList();
            return Task.FromResult(mapped);
        }

        public Task<List<MealSessionResponseModel>> GetAllMeallSessionByKitchenId(int kitchenid)
        {
            var result = _context.MealSessions
                .Include(x => x.Meal)
                .Include(x => x.Session).ThenInclude(x => x.SessionAreas).ThenInclude(x => x.Area).ThenInclude(x => x.District)
                .Include(x => x.Kitchen)
                .Where(x => x.KitchenId == kitchenid).Select(x => new MealSessionResponseModel
                {
                    MealSessionId = x.MealSessionId,
                    CreateDate = ((DateTime)x.CreateDate).ToString("dd-MM-yyyy"),
                    Price = (decimal?)x.Price,
                    Quantity = x.Quantity,
                    RemainQuantity = x.RemainQuantity,
                    Status = x.Status,
                    KitchenDtoForMealSession = new KitchenDtoForMealSession
                    {
                        KitchenId = x.Meal.Kitchen.KitchenId,
                        UserId = x.Meal.Kitchen.KitchenId,
                        Name = x.Meal.Kitchen.Name,
                        Address = x.Meal.Kitchen.Address,
                        AreaDtoForMealSession = new AreaDtoForMealSession
                        {
                            AreaId = x.Meal.Kitchen.Area.AreaId,
                            Address = x.Meal.Kitchen.Area.Address,
                            AreaName = x.Meal.Kitchen.Area.AreaName,
                        },
                    },
                    SessionDtoForMealSession = new SessionDtoForMealSession
                    {
                        SessionId = x.Session.SessionId,
                        CreateDate = ((DateTime)x.Session.CreateDate).ToString("dd-MM-yyyy"),
                        StartTime = ((DateTime)x.Session.StartTime).ToString("HH:mm"),
                        EndTime = ((DateTime)x.Session.EndTime).ToString("HH:mm"),
                        EndDate = ((DateTime)x.Session.EndDate).ToString("dd-MM-yyyy"),
                        UserId = x.Session.UserId,
                        Status = x.Session.Status,
                        SessionType = x.Session.SessionType,
                        SessionName = x.Session.SessionName,

                    },
                    MealDtoForMealSession = new MealDtoForMealSession
                    {
                        MealId = x.Meal.MealId,
                        Name = x.Meal.Name,
                        Image = x.Meal.Image,
                        KitchenId = x.KitchenId,
                        CreateDate = ((DateTime)x.Meal.CreateDate).ToString("dd-MM-yyyy"),
                        Description = x.Meal.Description,
                    },
                })
                .ToList();

            var mapped = result.Select(r => _mapper.Map<MealSessionResponseModel>(r)).ToList();
            return Task.FromResult(mapped);
        }

        public async Task<List<MealSessionResponseModel>> GetAllMeallSessionWithStatusAPPROVEDandREMAINQUANTITYhigherthan0InDay()
        {
            var datenow = GetDateTimeTimeZoneVietNam();
            var result = await _context.MealSessions
                .Include(x => x.Meal)
                .Include(x => x.Session).ThenInclude(x => x.SessionAreas).ThenInclude(x => x.Area).ThenInclude(x => x.District)
                .Include(x => x.Kitchen)
                .Where(x => x.Status == "APPROVED" && x.RemainQuantity > 0 && x.CreateDate == datenow)
                .Select(x => new MealSessionResponseModel
                {
                    MealSessionId = x.MealSessionId,
                    CreateDate = ((DateTime)x.CreateDate).ToString("dd-MM-yyyy"),
                    Price = (decimal?)x.Price,
                    Quantity = x.Quantity,
                    RemainQuantity = x.RemainQuantity,
                    Status = x.Status,
                    KitchenDtoForMealSession = new KitchenDtoForMealSession
                    {
                        KitchenId = x.Meal.Kitchen.KitchenId,
                        UserId = x.Meal.Kitchen.KitchenId,
                        Name = x.Meal.Kitchen.Name,
                        Address = x.Meal.Kitchen.Address,
                        AreaDtoForMealSession = new AreaDtoForMealSession
                        {
                            AreaId = x.Meal.Kitchen.Area.AreaId,
                            Address = x.Meal.Kitchen.Area.Address,
                            AreaName = x.Meal.Kitchen.Area.AreaName,
                        },

                    },
                    SessionDtoForMealSession = new SessionDtoForMealSession
                    {
                        SessionId = x.Session.SessionId,
                        CreateDate = ((DateTime)x.Session.CreateDate).ToString("dd-MM-yyyy"),
                        StartTime = ((DateTime)x.Session.StartTime).ToString("HH:mm"),
                        EndTime = ((DateTime)x.Session.EndTime).ToString("HH:mm"),
                        EndDate = ((DateTime)x.Session.EndDate).ToString("dd-MM-yyyy"),
                        UserId = x.Session.UserId,
                        Status = x.Session.Status,
                        SessionType = x.Session.SessionType,
                        SessionName = x.Session.SessionName,
                    },
                    MealDtoForMealSession = new MealDtoForMealSession
                    {
                        MealId = x.Meal.MealId,
                        Name = x.Meal.Name,
                        Image = x.Meal.Image,
                        KitchenId = x.KitchenId,
                        CreateDate = ((DateTime)x.Meal.CreateDate).ToString("dd-MM-yyyy"),
                        Description = x.Meal.Description,
                    },
                })
                .ToListAsync();

            var mapped = result.Select(r => _mapper.Map<MealSessionResponseModel>(r)).ToList();
            return mapped;

        }

        public Task<List<MealSessionResponseModel>> GetAllMeallSessionByKitchenIdInSession(int kitchenid, int sessionid)
        {
            var result = _context.MealSessions
                .Include(x => x.Meal)
                .Include(x => x.Session).ThenInclude(x => x.SessionAreas).ThenInclude(x => x.Area).ThenInclude(x => x.District)
                .Include(x => x.Kitchen)
                .Where(x => x.KitchenId == kitchenid && x.SessionId == sessionid)
                .Select(x => new MealSessionResponseModel
                {
                    MealSessionId = x.MealSessionId,
                    CreateDate = ((DateTime)x.CreateDate).ToString("dd-MM-yyyy"),
                    Price = (decimal?)x.Price,
                    Quantity = x.Quantity,
                    RemainQuantity = x.RemainQuantity,
                    Status = x.Status,
                    KitchenDtoForMealSession = new KitchenDtoForMealSession
                    {
                        KitchenId = x.Meal.Kitchen.KitchenId,
                        UserId = x.Meal.Kitchen.KitchenId,
                        Name = x.Meal.Kitchen.Name,
                        Address = x.Meal.Kitchen.Address,
                        AreaDtoForMealSession = new AreaDtoForMealSession
                        {
                            AreaId = x.Meal.Kitchen.Area.AreaId,
                            Address = x.Meal.Kitchen.Area.Address,
                            AreaName = x.Meal.Kitchen.Area.AreaName,
                        },

                    },
                    SessionDtoForMealSession = new SessionDtoForMealSession
                    {
                        SessionId = x.Session.SessionId,
                        CreateDate = ((DateTime)x.Session.CreateDate).ToString("dd-MM-yyyy"),
                        StartTime = ((DateTime)x.Session.StartTime).ToString("HH:mm"),
                        EndTime = ((DateTime)x.Session.EndTime).ToString("HH:mm"),
                        EndDate = ((DateTime)x.Session.EndDate).ToString("dd-MM-yyyy"),
                        UserId = x.Session.UserId,
                        Status = x.Session.Status,
                        SessionType = x.Session.SessionType,
                        SessionName = x.Session.SessionName,

                    },
                    MealDtoForMealSession = new MealDtoForMealSession
                    {
                        MealId = x.Meal.MealId,
                        Name = x.Meal.Name,
                        Image = x.Meal.Image,
                        KitchenId = x.KitchenId,
                        CreateDate = ((DateTime)x.Meal.CreateDate).ToString("dd-MM-yyyy"),
                        Description = x.Meal.Description,
                    },
                })
                .ToList();

            var mapped = result.Select(r => _mapper.Map<MealSessionResponseModel>(r)).ToList();
            return Task.FromResult(mapped);
        }

        public Task<List<MealSessionResponseModel>> GetAllMeallSessionBySessionIdWithStatusApprovedandREMAINQUANTITYhigherthan0InDay(int sessionid)
        {
            var datenow = GetDateTimeTimeZoneVietNam();
            var result = _context.MealSessions
                .Include(x => x.Meal)
                .Include(x => x.Session).ThenInclude(x => x.SessionAreas).ThenInclude(x => x.Area).ThenInclude(x => x.District)
                .Include(x => x.Kitchen)
                .Where(x => x.SessionId == sessionid && x.Status.Equals("APPROVED") && x.CreateDate == datenow && x.RemainQuantity > 0)
                .Select(x => new MealSessionResponseModel
                {
                    MealSessionId = x.MealSessionId,
                    CreateDate = ((DateTime)x.CreateDate).ToString("dd-MM-yyyy"),
                    Price = (decimal?)x.Price,
                    Quantity = x.Quantity,
                    RemainQuantity = x.RemainQuantity,
                    Status = x.Status,
                    KitchenDtoForMealSession = new KitchenDtoForMealSession
                    {
                        KitchenId = x.Meal.Kitchen.KitchenId,
                        UserId = x.Meal.Kitchen.KitchenId,
                        Name = x.Meal.Kitchen.Name,
                        Address = x.Meal.Kitchen.Address,
                        AreaDtoForMealSession = new AreaDtoForMealSession
                        {
                            AreaId = x.Meal.Kitchen.Area.AreaId,
                            Address = x.Meal.Kitchen.Area.Address,
                            AreaName = x.Meal.Kitchen.Area.AreaName,
                        }
                    },
                    SessionDtoForMealSession = new SessionDtoForMealSession
                    {
                        SessionId = x.Session.SessionId,
                        CreateDate = ((DateTime)x.Session.CreateDate).ToString("dd-MM-yyyy"),
                        StartTime = ((DateTime)x.Session.StartTime).ToString("HH:mm"),
                        EndTime = ((DateTime)x.Session.EndTime).ToString("HH:mm"),
                        EndDate = ((DateTime)x.Session.EndDate).ToString("dd-MM-yyyy"),
                        UserId = x.Session.UserId,
                        Status = x.Session.Status,
                        SessionType = x.Session.SessionType,
                        SessionName = x.Session.SessionName,

                    },
                    MealDtoForMealSession = new MealDtoForMealSession
                    {
                        MealId = x.Meal.MealId,
                        Name = x.Meal.Name,
                        Image = x.Meal.Image,
                        KitchenId = x.KitchenId,
                        CreateDate = ((DateTime)x.Meal.CreateDate).ToString("dd-MM-yyyy"),
                        Description = x.Meal.Description,
                    },
                })
                .ToList();

            var mapped = result.Select(r => _mapper.Map<MealSessionResponseModel>(r)).ToList();
            return Task.FromResult(mapped);
        }

        public Task<List<MealSessionResponseModel>> GetAllMeallSessionByKitchenIdWithStatusApprovedandREMAINQUANTITYhigherthan0InDay(int kitchenid)
        {
            var datenow = GetDateTimeTimeZoneVietNam();
            var result = _context.MealSessions
                .Include(x => x.Meal)
                .Include(x => x.Session).ThenInclude(x => x.SessionAreas).ThenInclude(x => x.Area).ThenInclude(x => x.District)
                .Include(x => x.Kitchen)
                .Where(x => x.KitchenId == kitchenid && x.Status.Equals("APPROVED") && x.CreateDate == datenow && x.RemainQuantity > 0)
                .Select(x => new MealSessionResponseModel
                {
                    MealSessionId = x.MealSessionId,
                    CreateDate = ((DateTime)x.CreateDate).ToString("dd-MM-yyyy"),
                    Price = (decimal?)x.Price,
                    Quantity = x.Quantity,
                    RemainQuantity = x.RemainQuantity,
                    Status = x.Status,
                    KitchenDtoForMealSession = new KitchenDtoForMealSession
                    {
                        KitchenId = x.Meal.Kitchen.KitchenId,
                        UserId = x.Meal.Kitchen.KitchenId,
                        Name = x.Meal.Kitchen.Name,
                        Address = x.Meal.Kitchen.Address,
                    },
                    SessionDtoForMealSession = new SessionDtoForMealSession
                    {
                        SessionId = x.Session.SessionId,
                        CreateDate = ((DateTime)x.Session.CreateDate).ToString("dd-MM-yyyy"),
                        StartTime = ((DateTime)x.Session.StartTime).ToString("HH:mm"),
                        EndTime = ((DateTime)x.Session.EndTime).ToString("HH:mm"),
                        EndDate = ((DateTime)x.Session.EndDate).ToString("dd-MM-yyyy"),
                        UserId = x.Session.UserId,
                        Status = x.Session.Status,
                        SessionType = x.Session.SessionType,
                        SessionName = x.Session.SessionName,

                    },
                    MealDtoForMealSession = new MealDtoForMealSession
                    {
                        MealId = x.Meal.MealId,
                        Name = x.Meal.Name,
                        Image = x.Meal.Image,
                        KitchenId = x.KitchenId,
                        CreateDate = ((DateTime)x.Meal.CreateDate).ToString("dd-MM-yyyy"),
                        Description = x.Meal.Description,
                    },
                })
                .ToList();

            var mapped = result.Select(r => _mapper.Map<MealSessionResponseModel>(r)).ToList();
            return Task.FromResult(mapped);
        }

        public Task<List<MealSessionResponseModel>> GetAllMeallSessionBySessionIdINDAYAndByKitchenId(int sessionid, int kitchenid)
        {
            var datenow = GetDateTimeTimeZoneVietNam();
            var result = _context.MealSessions
                .Include(x => x.Meal)
                .Include(x => x.Session).ThenInclude(x => x.SessionAreas).ThenInclude(x => x.Area).ThenInclude(x => x.District)
                .Include(x => x.Kitchen)
                .Where(x => x.SessionId == sessionid && x.CreateDate == datenow && x.KitchenId == kitchenid)
                .Select(x => new MealSessionResponseModel
                {
                    MealSessionId = x.MealSessionId,
                    CreateDate = ((DateTime)x.CreateDate).ToString("dd-MM-yyyy"),
                    Price = (decimal?)x.Price,
                    Quantity = x.Quantity,
                    RemainQuantity = x.RemainQuantity,
                    Status = x.Status,
                    KitchenDtoForMealSession = new KitchenDtoForMealSession
                    {
                        KitchenId = x.Meal.Kitchen.KitchenId,
                        UserId = x.Meal.Kitchen.KitchenId,
                        Name = x.Meal.Kitchen.Name,
                        Address = x.Meal.Kitchen.Address,
                    },
                    SessionDtoForMealSession = new SessionDtoForMealSession
                    {
                        SessionId = x.Session.SessionId,
                        CreateDate = ((DateTime)x.Session.CreateDate).ToString("dd-MM-yyyy"),
                        StartTime = ((DateTime)x.Session.StartTime).ToString("HH:mm"),
                        EndTime = ((DateTime)x.Session.EndTime).ToString("HH:mm"),
                        EndDate = ((DateTime)x.Session.EndDate).ToString("dd-MM-yyyy"),
                        UserId = x.Session.UserId,
                        Status = x.Session.Status,
                        SessionType = x.Session.SessionType,
                        SessionName = x.Session.SessionName,

                    },
                    MealDtoForMealSession = new MealDtoForMealSession
                    {
                        MealId = x.Meal.MealId,
                        Name = x.Meal.Name,
                        Image = x.Meal.Image,
                        KitchenId = x.KitchenId,
                        CreateDate = ((DateTime)x.Meal.CreateDate).ToString("dd-MM-yyyy"),
                        Description = x.Meal.Description,
                    },
                })
                .ToList();

            var mapped = result.Select(r => _mapper.Map<MealSessionResponseModel>(r)).ToList();
            return Task.FromResult(mapped);
        }

        public Task<List<MealSessionResponseModel>> GetAllMealSessionByAreaIdAndSessionIdAndKitchenId(int areaId, int sessionId, int kitchenId)
        {
            var getAreaId = _context.SessionAreas.Where(x => x.SessionId == sessionId && x.AreaId == areaId).Select(x => x.AreaId).FirstOrDefault();

            if (getAreaId != null)
            {
                var getListKitchen = _context.Kitchens.Where(x => x.AreaId == areaId).Select(x => x.KitchenId).ToList();

                if (getListKitchen.Count == 0)
                {
                    throw new Exception("Can not find kitchen");
                }

                var result = _context.MealSessions
                    .Where(x => x.SessionId == sessionId && x.KitchenId == kitchenId)
                    .Select(x => new MealSessionResponseModel
                    {
                        MealSessionId = x.MealSessionId,
                        CreateDate = ((DateTime)x.CreateDate).ToString("dd-MM-yyyy"),
                        Price = (decimal?)x.Price,
                        Quantity = x.Quantity,
                        RemainQuantity = x.RemainQuantity,
                        Status = x.Status,
                        KitchenDtoForMealSession = new KitchenDtoForMealSession
                        {
                            KitchenId = kitchenId,
                            UserId = x.Meal.Kitchen.KitchenId,
                            Name = x.Meal.Kitchen.Name,
                            Address = x.Meal.Kitchen.Address,
                        },
                        SessionDtoForMealSession = new SessionDtoForMealSession
                        {
                            SessionId = sessionId,
                            CreateDate = ((DateTime)x.Session.CreateDate).ToString("dd-MM-yyyy"),
                            StartTime = ((DateTime)x.Session.StartTime).ToString("HH:mm"),
                            EndTime = ((DateTime)x.Session.EndTime).ToString("HH:mm"),
                            EndDate = ((DateTime)x.Session.EndDate).ToString("dd-MM-yyyy"),
                            UserId = x.Session.UserId,
                            Status = x.Session.Status,
                            SessionType = x.Session.SessionType,
                            SessionName = x.Session.SessionName,
                            //AreaDtoForMealSession = new AreaDtoForMealSession
                            //{
                            //    AreaId = areaId,
                            //    Address = x.Session.SessionAreas.SingleOrDefault(sa => sa.SessionId == sessionId).Area.Address,
                            //    AreaName = x.Session.SessionAreas.SingleOrDefault(sa => sa.SessionId == sessionId).Area.AreaName,
                            //    DistrictDtoForMealSession = new DistrictDtoForMealSession
                            //    {
                            //        DistrictId = x.Session.SessionAreas.SingleOrDefault(sa => sa.SessionId == sessionId).Area.District.DistrictId,
                            //        DistrictName = x.Session.SessionAreas.SingleOrDefault(sa => sa.SessionId == sessionId).Area.District.DistrictName,
                            //    },
                            //},
                        },
                        MealDtoForMealSession = new MealDtoForMealSession
                        {
                            MealId = x.Meal.MealId,
                            Name = x.Meal.Name,
                            Image = x.Meal.Image,
                            KitchenId = x.KitchenId,
                            CreateDate = ((DateTime)x.Meal.CreateDate).ToString("dd-MM-yyyy"),
                            Description = x.Meal.Description,
                        },
                    }).ToList();
                var mapped = result.Select(r => _mapper.Map<MealSessionResponseModel>(r)).ToList();
                return Task.FromResult(mapped);
            }
            else
            {
                throw new Exception("Area is Null");
            }
            return null;

        }

        public async Task<List<MealSessionResponseModel>> GetAllMealSessionWithStatusProcessingByKitchenId(int kitchenId)
        {
            var result = _context.MealSessions
                .Include(x => x.Meal)
                .Include(x => x.Session)
                .Include(x => x.Kitchen)
                .Where(x => x.Status.Equals("PROCESSING") && x.KitchenId == kitchenId).Select(x => new MealSessionResponseModel
                {
                    MealSessionId = x.MealSessionId,
                    CreateDate = ((DateTime)x.CreateDate).ToString("dd-MM-yyyy"),
                    Price = (decimal?)x.Price,
                    Quantity = x.Quantity,
                    RemainQuantity = x.RemainQuantity,
                    Status = x.Status,
                    KitchenDtoForMealSession = new KitchenDtoForMealSession
                    {
                        KitchenId = x.Meal.Kitchen.KitchenId,
                        UserId = x.Meal.Kitchen.KitchenId,
                        Name = x.Meal.Kitchen.Name,
                        Address = x.Meal.Kitchen.Address,
                        AreaDtoForMealSession = new AreaDtoForMealSession
                        {
                            AreaId = x.Meal.Kitchen.Area.AreaId,
                            Address = x.Meal.Kitchen.Area.Address,
                            AreaName = x.Meal.Kitchen.Area.AreaName,
                        },
                    },
                    SessionDtoForMealSession = new SessionDtoForMealSession
                    {
                        SessionId = x.Session.SessionId,
                        CreateDate = ((DateTime)x.Session.CreateDate).ToString("dd-MM-yyyy"),
                        StartTime = ((DateTime)x.Session.StartTime).ToString("HH:mm"),
                        EndTime = ((DateTime)x.Session.EndTime).ToString("HH:mm"),
                        EndDate = ((DateTime)x.Session.EndDate).ToString("dd-MM-yyyy"),
                        UserId = x.Session.UserId,
                        Status = x.Session.Status,
                        SessionType = x.Session.SessionType,
                        SessionName = x.Session.SessionName,
                    },
                    MealDtoForMealSession = new MealDtoForMealSession
                    {
                        MealId = x.Meal.MealId,
                        Name = x.Meal.Name,
                        Image = x.Meal.Image,
                        KitchenId = x.KitchenId,
                        CreateDate = ((DateTime)x.Meal.CreateDate).ToString("dd-MM-yyyy"),
                        Description = x.Meal.Description,
                    },
                })
                .ToList();
            var mapped = result.Select(r => _mapper.Map<MealSessionResponseModel>(r)).ToList();
            return mapped;
        }

        public async Task<List<MealSessionResponseModel>> GetAllMealSessionByMealId(int mealId)
        {
            var datenow = GetDateTimeTimeZoneVietNam();
            var result = _context.MealSessions.Include(x => x.Session).Where(x => x.MealId == mealId && x.Session.EndDate.Value.Date == datenow.Date && x.Status.Equals("APPROVED")).Select(x => new MealSessionResponseModel
            {
                MealSessionId = x.MealSessionId,
                CreateDate = ((DateTime)x.CreateDate).ToString("dd-MM-yyyy"),
                Price = (decimal?)x.Price,
                Quantity = x.Quantity,
                RemainQuantity = x.RemainQuantity,
                Status = x.Status,
                KitchenDtoForMealSession = new KitchenDtoForMealSession
                {
                    KitchenId = x.Meal.Kitchen.KitchenId,
                    UserId = x.Meal.Kitchen.KitchenId,
                    Name = x.Meal.Kitchen.Name,
                    Address = x.Meal.Kitchen.Address,
                    AreaDtoForMealSession = new AreaDtoForMealSession
                    {
                        AreaId = x.Meal.Kitchen.Area.AreaId,
                        AreaName = x.Meal.Kitchen.Area.AreaName,
                        Address = x.Meal.Kitchen.Area.Address,
                    },
                },
                SessionDtoForMealSession = new SessionDtoForMealSession
                {
                    SessionId = x.Session.SessionId,
                    CreateDate = ((DateTime)x.Session.CreateDate).ToString("dd-MM-yyyy"),
                    StartTime = ((DateTime)x.Session.StartTime).ToString("HH:mm"),
                    EndTime = ((DateTime)x.Session.EndTime).ToString("HH:mm"),
                    EndDate = ((DateTime)x.Session.EndDate).ToString("dd-MM-yyyy"),
                    UserId = x.Session.UserId,
                    Status = x.Session.Status,
                    SessionType = x.Session.SessionType,
                    SessionName = x.Session.SessionName,
                },
                MealDtoForMealSession = new MealDtoForMealSession
                {
                    MealId = x.Meal.MealId,
                    Name = x.Meal.Name,
                    Image = x.Meal.Image,
                    KitchenId = x.KitchenId,
                    CreateDate = ((DateTime)x.Meal.CreateDate).ToString("dd-MM-yyyy"),
                    Description = x.Meal.Description,
                },
            }).ToList();
            var mapped = result.Select(r => _mapper.Map<MealSessionResponseModel>(r)).ToList();
            return mapped;
        }

        public async Task<List<MealSessionResponseModel>> GetAllMeallSessionWithNotStatusCompletedAndCancelledByKitchenId(int kitchenId)
        {
            var result = _context.MealSessions.Include(x => x.Session).Where(x => !x.Status.Equals("COMPLETED") && !x.Status.Equals("CANCELLED") && x.KitchenId == kitchenId).Select(x => new MealSessionResponseModel
            {
                MealSessionId = x.MealSessionId,
                CreateDate = ((DateTime)x.CreateDate).ToString("dd-MM-yyyy"),
                Price = (decimal?)x.Price,
                Quantity = x.Quantity,
                RemainQuantity = x.RemainQuantity,
                Status = x.Status,
                KitchenDtoForMealSession = new KitchenDtoForMealSession
                {
                    KitchenId = x.Meal.Kitchen.KitchenId,
                    UserId = x.Meal.Kitchen.KitchenId,
                    Name = x.Meal.Kitchen.Name,
                    Address = x.Meal.Kitchen.Address,
                    AreaDtoForMealSession = new AreaDtoForMealSession
                    {
                        AreaId = x.Meal.Kitchen.Area.AreaId,
                        AreaName = x.Meal.Kitchen.Area.AreaName,
                        Address = x.Meal.Kitchen.Area.Address,
                    },
                },
                SessionDtoForMealSession = new SessionDtoForMealSession
                {
                    SessionId = x.Session.SessionId,
                    CreateDate = ((DateTime)x.Session.CreateDate).ToString("dd-MM-yyyy"),
                    StartTime = ((DateTime)x.Session.StartTime).ToString("HH:mm"),
                    EndTime = ((DateTime)x.Session.EndTime).ToString("HH:mm"),
                    EndDate = ((DateTime)x.Session.EndDate).ToString("dd-MM-yyyy"),
                    UserId = x.Session.UserId,
                    Status = x.Session.Status,
                    SessionType = x.Session.SessionType,
                    SessionName = x.Session.SessionName,
                },
                MealDtoForMealSession = new MealDtoForMealSession
                {
                    MealId = x.Meal.MealId,
                    Name = x.Meal.Name,
                    Image = x.Meal.Image,
                    KitchenId = x.KitchenId,
                    CreateDate = ((DateTime)x.Meal.CreateDate).ToString("dd-MM-yyyy"),
                    Description = x.Meal.Description,
                },
            }).ToList();

            var mapped = result.Select(r => _mapper.Map<MealSessionResponseModel>(r)).ToList();
            return mapped;
        }

        public async Task UpdateAreaAndAllMealSessionWithStatusProcessing(UpdateAreaAndAllMealSessionWithStatusProcessingRequestModel request)
        {
            var result = _context.MealSessions.Where(x => request.mealSessionIds.Contains(x.MealSessionId) && x.Status.Equals("PROCESSING")).ToList();
            if (result != null)
            {
                foreach (var r in result)
                {
                    r.Status = "CANCELLEDBYCHEF";
                }
            }
            else
            {
                throw new Exception("Can not find meal session ID");
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<MealSessionResponseModel>> GetAllMeallSessionWithStatusCompletedByKitchenId(int kitchenId)
        {
            var datenow = GetDateTimeTimeZoneVietNam();
            var result = _context.MealSessions.Include(x => x.Session).Where(x => x.KitchenId == kitchenId && x.Status.Equals("COMPLETED")).Select(x => new MealSessionResponseModel
            {
                MealSessionId = x.MealSessionId,
                CreateDate = ((DateTime)x.CreateDate).ToString("dd-MM-yyyy"),
                Price = (decimal?)x.Price,
                Quantity = x.Quantity,
                RemainQuantity = x.RemainQuantity,
                Status = x.Status,
                KitchenDtoForMealSession = new KitchenDtoForMealSession
                {
                    KitchenId = x.Meal.Kitchen.KitchenId,
                    UserId = x.Meal.Kitchen.KitchenId,
                    Name = x.Meal.Kitchen.Name,
                    Address = x.Meal.Kitchen.Address,
                    AreaDtoForMealSession = new AreaDtoForMealSession
                    {
                        AreaId = x.Meal.Kitchen.Area.AreaId,
                        AreaName = x.Meal.Kitchen.Area.AreaName,
                        Address = x.Meal.Kitchen.Area.Address,
                    },
                },
                SessionDtoForMealSession = new SessionDtoForMealSession
                {
                    SessionId = x.Session.SessionId,
                    CreateDate = ((DateTime)x.Session.CreateDate).ToString("dd-MM-yyyy"),
                    StartTime = ((DateTime)x.Session.StartTime).ToString("HH:mm"),
                    EndTime = ((DateTime)x.Session.EndTime).ToString("HH:mm"),
                    EndDate = ((DateTime)x.Session.EndDate).ToString("dd-MM-yyyy"),
                    UserId = x.Session.UserId,
                    Status = x.Session.Status,
                    SessionType = x.Session.SessionType,
                    SessionName = x.Session.SessionName,
                },
                MealDtoForMealSession = new MealDtoForMealSession
                {
                    MealId = x.Meal.MealId,
                    Name = x.Meal.Name,
                    Image = x.Meal.Image,
                    KitchenId = x.KitchenId,
                    CreateDate = ((DateTime)x.Meal.CreateDate).ToString("dd-MM-yyyy"),
                    Description = x.Meal.Description,
                },
            }).ToList();
            var mapped = result.Select(r => _mapper.Map<MealSessionResponseModel>(r)).ToList();
            return mapped;
        }
    }
}

