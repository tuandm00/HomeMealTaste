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
                .Include(x => x.Session).ThenInclude(x => x.SessionAreas).ThenInclude(x => x.Area)
                .Include(x => x.Kitchen).Select(x => new MealSessionResponseModel
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
                        AreaDtoForMealSession = new AreaDtoForMealSession
                        {
                            AreaId = (int)(x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.AreaId),
                            Address = x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.Address,
                            AreaName = x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.AreaName,
                        },
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
                        AreaDtoForMealSession = new AreaDtoForMealSession
                        {
                            AreaId = (int)(x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.AreaId),
                            Address = x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.Address,
                            AreaName = x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.AreaName,
                        },
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
            AreaDtoForMealSessions = new AreaDtoForMealSessions
            {
                AreaId = (int)(gr.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == gr.Session.SessionId).Area.AreaId),
                Address = gr.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == gr.Session.SessionId).Area.Address,
                AreaName = gr.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == gr.Session.SessionId).Area.AreaName,
                DistrictDtoForMealSessions = new DistrictDtoForMealSessions
                {
                    DistrictId = gr.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == gr.Session.SessionId).Area.District.DistrictId,
                    DistrictName = gr.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == gr.Session.SessionId).Area.District.DistrictName,
                },
            },
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

        public async Task UpdateStatusMeallSession(int mealsessionid, string status)
        {
            var datenow = GetDateTimeTimeZoneVietNam();
            var sessionId = _context.MealSessions.Where(x => x.MealSessionId == mealsessionid).Select(x => x.SessionId).FirstOrDefault();
            var sesstioStatus = _context.Sessions.Where(x => x.SessionId == sessionId).Select(x => x.Status).FirstOrDefault();
            var result = await _context.MealSessions.SingleOrDefaultAsync(x => x.MealSessionId == mealsessionid);
            if (result.CreateDate.Value.Date != datenow.Date)
            {
                throw new Exception("Can not Update Because Not In Day");
            }
            else
            {
                if (result != null && result.Status.Equals("PROCESSING", StringComparison.OrdinalIgnoreCase) && sesstioStatus == true)
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
                else if (result != null && result.Status.Equals("APPROVED", StringComparison.OrdinalIgnoreCase) && sesstioStatus == true)
                {
                    if (string.Equals("APPROVED", status, StringComparison.OrdinalIgnoreCase))
                    {
                        result.Status = "APPROVED";
                    }
                    else result.Status = "REJECTED";
                }

                else if (result != null && result.Status.Equals("REJECTED", StringComparison.OrdinalIgnoreCase) && sesstioStatus == true)
                {
                    if (string.Equals("REJECTED", status, StringComparison.OrdinalIgnoreCase))
                    {
                        result.Status = "REJECTED";
                    }
                    else result.Status = "APPROVED";
                }
                else throw new Exception("Session is OFF");

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
                    AreaDtoForMealSession = new AreaDtoForMealSession
                    {
                        AreaId = (int)(mealsession.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == mealsession.Session.SessionId).Area.AreaId),
                        Address = mealsession.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == mealsession.Session.SessionId).Area.Address,
                        AreaName = mealsession.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == mealsession.Session.SessionId).Area.AreaName,
                        DistrictDtoForMealSession = new DistrictDtoForMealSession
                        {
                            DistrictId = (int)(mealsession.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == mealsession.Session.SessionId).Area.District.DistrictId),
                            DistrictName = (mealsession.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == mealsession.Session.SessionId).Area.District.DistrictName),
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
                        AreaDtoForMealSession = new AreaDtoForMealSession
                        {
                            AreaId = (int)(x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.AreaId),
                            Address = x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.Address,
                            AreaName = x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.AreaName,
                            DistrictDtoForMealSession = new DistrictDtoForMealSession
                            {
                                DistrictId = x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.District.DistrictId,
                                DistrictName = x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.District.DistrictName,
                            },
                        },
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
                        AreaDtoForMealSession = new AreaDtoForMealSession
                        {
                            AreaId = (int)(x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.AreaId),
                            Address = x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.Address,
                            AreaName = x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.AreaName,
                            DistrictDtoForMealSession = new DistrictDtoForMealSession
                            {
                                DistrictId = x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.District.DistrictId,
                                DistrictName = x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.District.DistrictName,
                            },
                        },
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
                        AreaDtoForMealSession = new AreaDtoForMealSession
                        {
                            AreaId = (int)(x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.AreaId),
                            Address = x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.Address,
                            AreaName = x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.AreaName,
                            DistrictDtoForMealSession = new DistrictDtoForMealSession
                            {
                                DistrictId = x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.District.DistrictId,
                                DistrictName = x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.District.DistrictName,
                            },
                        },
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
                        AreaDtoForMealSession = new AreaDtoForMealSession
                        {
                            AreaId = (int)(x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.AreaId),
                            Address = x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.Address,
                            AreaName = x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.AreaName,
                            DistrictDtoForMealSession = new DistrictDtoForMealSession
                            {
                                DistrictId = x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.District.DistrictId,
                                DistrictName = x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.District.DistrictName,
                            },
                        },
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
                        AreaDtoForMealSession = new AreaDtoForMealSession
                        {
                            AreaId = (int)(x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.AreaId),
                            Address = x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.Address,
                            AreaName = x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.AreaName,
                            DistrictDtoForMealSession = new DistrictDtoForMealSession
                            {
                                DistrictId = x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.District.DistrictId,
                                DistrictName = x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.District.DistrictName,
                            },
                        },
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
                        AreaDtoForMealSession = new AreaDtoForMealSession
                        {
                            AreaId = (int)(x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.AreaId),
                            Address = x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.Address,
                            AreaName = x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.AreaName,
                            DistrictDtoForMealSession = new DistrictDtoForMealSession
                            {
                                DistrictId = x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.District.DistrictId,
                                DistrictName = x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.District.DistrictName,
                            },
                        },
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
                        AreaDtoForMealSession = new AreaDtoForMealSession
                        {
                            AreaId = (int)(x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.AreaId),
                            Address = x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.Address,
                            AreaName = x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.AreaName,
                            DistrictDtoForMealSession = new DistrictDtoForMealSession
                            {
                                DistrictId = x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.District.DistrictId,
                                DistrictName = x.Session.SessionAreas.FirstOrDefault(sa => sa.SessionId == x.Session.SessionId).Area.District.DistrictName,
                            },
                        },
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

            if(getAreaId != null)
            {
                var getListKitchen = _context.Kitchens.Where(x => x.AreaId == areaId).Select(x => x.KitchenId).ToList();
                foreach(var item in getListKitchen)
                {
                    if(item != kitchenId)
                    {
                        throw new Exception("Can not find kitchen");
                    }
                };
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
    }
}

