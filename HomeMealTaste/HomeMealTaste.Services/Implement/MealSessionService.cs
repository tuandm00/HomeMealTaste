using AutoMapper;
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
            entity.CreateDate = GetDateTimeTimeZoneVietNam();
            entity.Quantity = mealSessionRequest.Quantity;
            entity.RemainQuantity = entity.Quantity;
            var result = await _mealSessionRepository.Create(entity, true);


            await _context.Entry(result).Reference(m => m.Meal).LoadAsync();
            await _context.Entry(result).Reference(m => m.Session).LoadAsync();
            await _context.Entry(result.Meal).Reference(s => s.Kitchen).LoadAsync();

            var mapped = _mapper.Map<MealSessionResponseModel>(result);
            mapped.MealSessionId = result.MealSessionId;
            mapped.Price = (decimal?)result.Price;
            mapped.Quantity = entity.Quantity;
            mapped.RemainQuantity = mapped.Quantity;
            mapped.Status = entity.Status;
            mapped.CreateDate = entity.CreateDate.ToString();
            mapped.MealDtoForMealSession = new MealDtoForMealSession
            {
                MealId = result.Meal.MealId,
                Name = result.Meal.Name,
                Image = result.Meal.Image,
                KitchenId = result.Meal.KitchenId,
                CreateDate = ((DateTime)result.Meal.CreateDate).ToString("dd-MM-yyyy"),
                Description = result.Meal.Description,
            };
            mapped.SessionDtoForMealSession = new SessionDtoForMealSession
            {
                SessionId = result.Session.SessionId,
                CreateDate = ((DateTime)result.Session.CreateDate).ToString("dd-MM-yyyy"),
                StartTime = ((DateTime)result.Session.StartTime).ToString("HH:mm"),
                EndTime = ((DateTime)result.Session.EndTime).ToString("HH:mm"),
                EndDate = ((DateTime)result.Session.EndDate).ToString("dd-MM-yyyy"),
                UserId = result.Session?.UserId,
                Status = result.Session.Status,
                SessionType = result.Session.SessionType,
            };
            mapped.KitchenDtoForMealSession = new KitchenDtoForMealSession
            {
                KitchenId = result.Meal.Kitchen.KitchenId,
                UserId = result.Meal.Kitchen.UserId,
                Name = result.Meal.Kitchen.Name,
                Address = result.Meal.Kitchen.Address,
            };

            return mapped;
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
                response.CreateDate = ((DateTime)mealsession.CreateDate).ToString("dd-MM-yyyy");

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
                response.CreateDate = ((DateTime)mealsession.CreateDate).ToString("dd-MM-yyyy");

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
                CreateDate = ((DateTime)result.Meal.CreateDate).ToString("dd-MM-yyyy"),
                Description = result.Meal?.Description,
            };
            mapped.SessionDtoForMealSession = new SessionDtoForMealSession
            {
                SessionId = result.Session.SessionId,
                CreateDate = ((DateTime)result.Session.CreateDate).ToString("dd-MM-yyyy"),
                StartTime = ((DateTime)result.Session.StartTime).ToString("HH:mm"),
                EndTime = ((DateTime)result.Session.EndTime).ToString("HH:mm"),
                EndDate = ((DateTime)result.Session.EndDate).ToString("dd-MM-yyyy"),
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
            mapped.CreateDate = ((DateTime)result.CreateDate).ToString("dd-MM-yyyy");

            return mapped;

        }

        public async Task UpdateStatusMeallSession(int mealsessionid, string status)
        {
            var result = await _context.MealSessions.SingleOrDefaultAsync(x => x.MealSessionId == mealsessionid);

            if (result != null && result.Status.Equals("PROCESSING", StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals("APPROVED", status, StringComparison.OrdinalIgnoreCase))
                {
                    result.Status = "APPROVED";
                }
                else if (string.Equals("REJECTED", status, StringComparison.OrdinalIgnoreCase))
                {
                    result.Status = "REJECTED";
                }
            }
            await _context.SaveChangesAsync();
            if (result != null && result.Status.Equals("APPROVED", StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals("APPROVED", status, StringComparison.OrdinalIgnoreCase))
                {
                    result.Status = "APPROVED";
                }
                else result.Status = "REJECTED";
            }
            await _context.SaveChangesAsync();

            if (result != null && result.Status.Equals("REJECTED", StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals("REJECTED", status, StringComparison.OrdinalIgnoreCase))
                {
                    result.Status = "REJECTED";
                }
                else result.Status = "APPROVED";
            }

            await _context.SaveChangesAsync();
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

        public Task<List<MealSessionResponseModel>> GetAllMeallSessionBySessionIdINDAY(int sessionid)
        {
            var datenow = GetDateTimeTimeZoneVietNam();
            var result = _context.MealSessions
                .Include(x => x.Meal)
                .Include(x => x.Session).ThenInclude(x => x.Area).ThenInclude(x => x.District)
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
                    AreaDtoForMealSession = new AreaDtoForMealSession
                    {
                        AreaId = mealsession.Session.Area.AreaId,
                        AreaName = mealsession.Session.Area.AreaName,
                        Address = mealsession.Session.Area.Address,
                        DistrictDtoForMealSession = new DistrictDtoForMealSession
                        {
                            DistrictId = mealsession.Session.Area.District.DistrictId,
                            DistrictName = mealsession.Session.Area.District.DistrictName
                        },
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
                .Include(x => x.Session).ThenInclude(x => x.Area).ThenInclude(x => x.District)
                .Include(x => x.Kitchen)
                .Where(x => x.SessionId == sessionid)
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
                    AreaDtoForMealSession = new AreaDtoForMealSession
                    {
                        AreaId = mealsession.Session.Area.AreaId,
                        AreaName = mealsession.Session.Area.AreaName,
                        Address = mealsession.Session.Area.Address,
                        DistrictDtoForMealSession = new DistrictDtoForMealSession
                        {
                            DistrictId = mealsession.Session.Area.District.DistrictId,
                            DistrictName = mealsession.Session.Area.District.DistrictName
                        },
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
                response.CreateDate = ((DateTime)mealsession.CreateDate).ToString("dd-MM-yyyy");

                return response;
            }).ToList();

            return Task.FromResult(mapped);
        }

        public Task<List<MealSessionResponseModel>> GetAllMeallSessionByKitchenId(int kitchenid)
        {
            var result = _context.MealSessions
                .Include(x => x.Meal)
                .Include(x => x.Session).ThenInclude(x => x.Area).ThenInclude(x => x.District)
                .Include(x => x.Kitchen)
                .Where(x => x.KitchenId == kitchenid)
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
                    AreaDtoForMealSession = new AreaDtoForMealSession
                    {
                        AreaId = mealsession.Session.Area.AreaId,
                        AreaName = mealsession.Session.Area.AreaName,
                        Address = mealsession.Session.Area.Address,
                        DistrictDtoForMealSession = new DistrictDtoForMealSession
                        {
                            DistrictId = mealsession.Session.Area.District.DistrictId,
                            DistrictName = mealsession.Session.Area.District.DistrictName
                        },
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
                response.CreateDate = ((DateTime)mealsession.CreateDate).ToString("dd-MM-yyyy");

                return response;
            }).ToList();

            return Task.FromResult(mapped);
        }

        public async Task<List<MealSessionResponseModel>> GetAllMeallSessionWithStatusAPPROVEDandREMAINQUANTITYhigherthan0InDay()
        {
            var datenow = GetDateTimeTimeZoneVietNam();
            var result = await _context.MealSessions
                .Include(x => x.Meal)
                .Include(x => x.Session).ThenInclude(x => x.Area)
                .Include(x => x.Kitchen)
                .Where(x => x.Status == "APPROVED" && x.RemainQuantity > 0 && x.CreateDate == datenow)
                .ToListAsync();

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
                response.CreateDate = ((DateTime)mealsession.CreateDate).ToString("dd-MM-yyyy");

                return response;
            }).ToList();

            return mapped;

        }
    }
}

