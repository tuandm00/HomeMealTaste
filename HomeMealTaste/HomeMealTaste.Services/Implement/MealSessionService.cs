﻿using AutoMapper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Globalization;

namespace HomeMealTaste.Services.Implement
{
    public class MealSessionService : IMealSessionService
    {
        private readonly IMealSessionRepository _mealSessionRepository;
        private readonly IMapper _mapper;
        private readonly IDishRepository _dishRepository;
        private readonly IMealRepository _mealRepository;
        private readonly IMealDishRepository _mealDishRepository;
        private readonly HomeMealTasteContext _context;

        public MealSessionService(IMealSessionRepository mealSessionRepository, IMapper mapper,
            IDishRepository dishRepository, IMealRepository mealRepository, IMealDishRepository mealDishRepository, HomeMealTasteContext context)
        {
            _mealSessionRepository = mealSessionRepository;
            _mapper = mapper;
            _dishRepository = dishRepository;
            _mealRepository = mealRepository;
            _mealDishRepository = mealDishRepository;
            _context = context; 
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
        public async Task<MealSessionResponseModel> CreateMealSession(MealSessionRequestModel mealSessionRequest)
        {
            var entity = _mapper.Map<MealSession>(mealSessionRequest);
            entity.Status = "PROCESSING";
            //entity.CreateDate = GetDateTimeTimeZoneVietNam().ToString("dd-MM-yyyy");

            var result = await _mealSessionRepository.Create(entity, true);

            return _mapper.Map<MealSessionResponseModel>(result);
        }

        public async Task<List<MealSessionResponseModel>> GetAllMealSession()
        {
            var result = _context.MealSessions
                .Include(x => x.Meal)
                .Include(x => x.Session).ThenInclude(x => x.Area)
                .Include(x => x.Kitchen)
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
                    CreateDate = mealsession.CreateDate.ToString(),
                    Description = mealsession.Meal?.Description,
                };
                response.SessionDtoForMealSession = new SessionDtoForMealSession
                {
                    SessionId = mealsession.Session.SessionId,
                    CreateDate = mealsession.Session.CreateDate.ToString(),
                    StartTime = mealsession.Session.StartTime.ToString(),
                    EndTime = mealsession.Session.EndTime.ToString(),
                    EndDate = mealsession.Session.EndDate.ToString(),
                    UserId = mealsession.Session?.UserId,
                    Status = mealsession.Session.Status,
                    SessionType = mealsession.Session.SessionType,
                    AreaDtoForMealSession = new AreaDtoForMealSession
                    {
                        AreaId = mealsession.Session.Area.AreaId,
                        Address = mealsession.Session.Area.Address,
                        AreaName = mealsession.Session.Area.AreaName,
                    },
                };
                response.KitchenDtoForMealSession = new KitchenDtoForMealSession
                {
                    KitchenId = mealsession.Meal.Kitchen.KitchenId,
                    UserId = mealsession.Meal.Kitchen.KitchenId,
                    Name = mealsession.Meal.Kitchen?.Name,
                    Address = mealsession.Meal.Kitchen.Address,
                };
                response.Price = (decimal?)mealsession.Price;
                response.Quantity = mealsession.Quantity;
                response.RemainQuantity = mealsession.RemainQuantity;
                response.Status = mealsession.Status;
                response.CreateDate = mealsession.CreateDate.ToString();
                
                return response;
            }).ToList();

            return mapped; 
        }

        public async Task<List<MealSessionResponseModel>> GetAllMealSessionByStatus(string status)
        {
            var lowerStatus = status.ToLower();
            var result = _context.MealSessions
                .Include(x => x.Meal)
                .Include(x => x.Session)
                .Include(x => x.Kitchen)
                .Where(x => x.Status.ToLower() == lowerStatus)
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
                    CreateDate = mealsession.CreateDate.ToString(),
                    Description = mealsession.Meal?.Description,
                };
                response.SessionDtoForMealSession = new SessionDtoForMealSession
                {
                    SessionId = mealsession.Session.SessionId,
                    CreateDate = mealsession.Session.CreateDate.ToString(),
                    StartTime = mealsession.Session.StartTime.ToString(),
                    EndTime = mealsession.Session.EndTime.ToString(),
                    EndDate = mealsession.Session.EndDate.ToString(),
                    UserId = mealsession.Session?.UserId,
                    Status = mealsession.Session.Status,
                    SessionType = mealsession.Session.SessionType,


                };
                response.KitchenDtoForMealSession = new KitchenDtoForMealSession
                {
                    KitchenId = mealsession.Meal.Kitchen.KitchenId,
                    UserId = mealsession.Meal.Kitchen.KitchenId,
                    Name = mealsession.Meal.Kitchen?.Name,
                    Address = mealsession.Meal.Kitchen.Address,
                };
                response.Price = (decimal?)mealsession.Price;
                response.Quantity = mealsession.Quantity;
                response.RemainQuantity = mealsession.RemainQuantity;
                response.Status = mealsession.Status;
                response.CreateDate = mealsession.CreateDate.ToString();

                return response;
            }).ToList();

            return mapped;
        }

        public async Task<MealSessionResponseModel> GetSingleMealSessionById(int mealsessionid)
        {
            var result = _context.MealSessions
                .Include(x => x.Meal)
                .Include(x => x.Session).ThenInclude(x => x.Area).ThenInclude(x => x.District)
                .Include(x => x.Kitchen)
                .Where(x => x.MealSessionId == mealsessionid)
                .FirstOrDefault();
            var mapped = _mapper.Map<MealSessionResponseModel>(result);
            mapped.MealSessionId = result.MealSessionId;
            mapped.MealDtoForMealSession = new MealDtoForMealSession
            {
                MealId = result.Meal.MealId,
                Name = result.Meal.Name,
                Image = result.Meal.Image,
                KitchenId = result.KitchenId,
                CreateDate = result.CreateDate.ToString(),
                Description = result.Meal?.Description,
            };
            mapped.SessionDtoForMealSession = new SessionDtoForMealSession
            {
                SessionId = result.Session.SessionId,
                CreateDate = result.Session.CreateDate.ToString(),
                StartTime = result.Session.StartTime.ToString(),
                EndTime = result.Session.EndTime.ToString(),
                EndDate = result.Session.EndDate.ToString(),
                UserId = result.Session?.UserId,
                Status = result.Session.Status,
                SessionType = result.Session.SessionType,
                AreaDtoForMealSession = new AreaDtoForMealSession
                {
                    AreaId = result.Session.Area.AreaId,
                    Address = result.Session.Area.Address,
                    AreaName = result.Session.Area.AreaName,
                    DistrictDtoForMealSession = new DistrictDtoForMealSession
                    {
                        DistrictId = (int)result.Session.Area.District.DistrictId,
                        DistrictName = result.Session.Area.District.DistrictName,
                    }
                },
            };
            mapped.KitchenDtoForMealSession = new KitchenDtoForMealSession
            {
                KitchenId = result.Meal.Kitchen.KitchenId,
                UserId = result.Meal.Kitchen.KitchenId,
                Name = result.Meal.Kitchen?.Name,
                Address = result.Meal.Kitchen.Address,
            };
            mapped.Price = (decimal?)result.Price;
            mapped.Quantity = result.Quantity;
            mapped.RemainQuantity = result.RemainQuantity;
            mapped.Status = result.Status;
            mapped.CreateDate = result.CreateDate.ToString();

            return mapped;

        }

        public Task UpdateStatusMeallSession(int mealsessionid, string status)
        {
            var result = _context.MealSessions.FirstOrDefault(x => x.MealSessionId == mealsessionid && x.Status == "PROCESSING");

            if (result != null)
            {
                if (status.Equals("APPROVED", StringComparison.OrdinalIgnoreCase))
                {
                    result.Status = "APPROVED";
                }
                else if (status.Equals("REJECTED", StringComparison.OrdinalIgnoreCase))
                {
                    result.Status = "REJECTED";
                }

                 _context.SaveChangesAsync();
            }
            return Task.FromResult(result);
        }

        //public async Task<PagedList<GetAllMealInCurrentSessionResponseModel>> GetAllMealSession(
        //    GetAllMealRequest pagingParams)
        //{
        //    var selectExpression = GetAllMealInCurrentSessionResponseModel.FromEntity();
        //    var includes = new Expression<Func<MealSession, object>>[]
        //    {
        //        x => x.Meal!,
        //        x => x.Session!,
        //        x => x.Meal.MealDishes,
        //    };
        //    Expression<Func<MealSession, bool>> conditionAddition = e => e.Session.StartTime < (pagingParams.SessionStartTime ?? DateTime.Now);

        //    var result =
        //        await _mealSessionRepository.GetWithPaging(pagingParams, conditionAddition, selectExpression, includes);
        //    foreach (var response in result.Data)
        //    {
        //        var mealId = response.Meal?.Id;
        //        if (mealId == null) continue;
        //        var meal = await _mealRepository.GetFirstOrDefault(x => x.MealId == mealId);

        //        var mealDishes = await _mealDishRepository.GetByCondition(x => x.MealId == meal.MealId);

        //        var dishes = new List<Dish>();
        //        foreach (var dishId in mealDishes)
        //        {
        //            var dish = await _dishRepository.GetFirstOrDefault(x => x.DishId == dishId.DishId, x => x.Kitchen);
        //            var kitchen =_mapper.Map<GetAllMealInCurrentSessionResponseModel.ChefInfo>(dish.Kitchen);
        //            response.Chef = kitchen;

        //            dishes.Add(dish);
        //        }
        //        response.Dish = _mapper.Map<List<GetAllMealInCurrentSessionResponseModel.DishModel?>>(dishes);
        //    }

        //    return result;
        //}


    }
}

