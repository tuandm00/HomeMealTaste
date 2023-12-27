using AutoMapper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Services.Interface;
using HomeMealTaste.Services.ResponseModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HomeMealTaste.Data.Helper;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HomeMealTaste.Services.Implement
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly ITransactionService _transactionService;
        private readonly HomeMealTasteContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<SessionService> _logger;
        public SessionService(HomeMealTasteContext context, ISessionRepository sessionRepository, IMapper mapper, ITransactionService transactionService, ILogger<SessionService> logger)
        {
            _context = context;
            _sessionRepository = sessionRepository;
            _mapper = mapper;
            _transactionService = transactionService;
            _logger = logger;
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
        private async Task<bool> SessionTypeExistsInAreaInDayNow(List<int> areaId, string sessionType)
        {
            var date = GetDateTimeTimeZoneVietNam().AddDays(1);
            foreach (var area in areaId)
            {
                var sessionId = _context.SessionAreas.Where(x => x.AreaId == area).Select(x => x.SessionId).ToList();
                foreach (var session in sessionId)
                {
                    var sessionTypes = _context.Sessions.Where(x => x.SessionId == session && x.EndDate == date).Select(x => x.SessionType).ToList();
                    foreach (var type in sessionTypes)
                    {
                        if ((type.ToLower()).Equals(sessionType))
                        {
                            return false;
                        }
                    }
                }

            }
            return true;

        }
        public async Task<SessionResponseModel> CreateSession(SessionRequestModel sessionRequest)
        {
            var responseModel = new SessionResponseModel(); // Initialize the response model

            try
            {
                var entity = _mapper.Map<Session>(sessionRequest);
                var sessionTypeLower = entity.SessionType.ToLower();


                if (sessionRequest.AreaIds != null)
                {
                    if (await SessionTypeExistsInAreaInDayNow(sessionRequest.AreaIds, sessionTypeLower))
                    {
                        SetSessionProperties(entity, sessionTypeLower, sessionRequest.AreaIds);
                        entity.EndDate = GetDateTimeTimeZoneVietNam();
                        entity.SessionName = $"Session: {entity.SessionType} , In: {((DateTime)entity.EndDate).ToString("dd-MM-yyyy")}";

                        var result = await _sessionRepository.Create(entity, true);

                        if (result != null && sessionRequest.AreaIds != null)
                        {
                            var uniqueAreaIds = new HashSet<int>();

                            foreach (var areaId in sessionRequest.AreaIds)
                            {
                                if (uniqueAreaIds.Add(areaId))
                                {
                                    var sessionArea = new SessionArea
                                    {
                                        SessionId = result.SessionId,
                                        AreaId = areaId,
                                    };
                                    await _context.AddAsync(sessionArea);
                                }
                                await _context.SaveChangesAsync();
                            }

                        }
                        responseModel = _mapper.Map<SessionResponseModel>(result);

                        responseModel.StartTime = result.StartTime?.ToString("HH:mm");
                        responseModel.EndTime = result.EndTime?.ToString("HH:mm");
                        responseModel.CreateDate = result.CreateDate?.ToString("dd-MM-yyyy");
                        responseModel.EndDate = result.EndDate?.ToString("dd-MM-yyyy");

                        responseModel.Message = "Success";
                    }
                    else
                    {
                        responseModel.Message = "Error: sessionType is EXISTED";
                    }
                }
                else
                {
                    responseModel.Message = "Error: Not Exist Area to Create Session";
                }

                return responseModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in CreateSession: {Message}", ex.Message);

                responseModel.Message = "Error: " + ex.Message;
                return responseModel;
            }
        }

        private void SetSessionProperties(Session entity, string sessionTypeLower, List<int> areaId)
        {
            entity.CreateDate = GetDateTimeTimeZoneVietNam();

            if (string.Equals(sessionTypeLower, "lunch", StringComparison.OrdinalIgnoreCase))
            {
                entity.StartTime = entity.CreateDate?.Date.AddHours(10);
                entity.EndTime = entity.StartTime?.AddHours(2);
                entity.SessionType = "Lunch";

            }
            else if (string.Equals(sessionTypeLower, "evening", StringComparison.OrdinalIgnoreCase))
            {
                entity.StartTime = entity.CreateDate?.Date.AddHours(16);
                entity.EndTime = entity.StartTime?.AddHours(4);
                entity.SessionType = "Evening";
            }
            else if (string.Equals(sessionTypeLower, "dinner", StringComparison.OrdinalIgnoreCase))
            {
                entity.StartTime = entity.CreateDate?.Date.AddHours(17);
                entity.EndTime = entity.StartTime?.AddHours(2);
                entity.SessionType = "Dinner";
            }

            entity.Status = true;
            entity.UserId = 2;
            entity.BookingSlotStatus = true;
            entity.RegisterForMealStatus = true;
            
        }


        //public async Task<SessionResponseModel> UpdateEndTime(int sessionId, DateTime endTime)
        //{
        //    var entity = await _sessionRepository.GetFirstOrDefault(x => x.SessionId == sessionId);
        //    entity.EndTime = endTime;

        //    var response = await _sessionRepository.Update(entity);

        //    return _mapper.Map<SessionResponseModel>(response);

        //}

        //public async Task<PagedList<GetAllMealInCurrentSessionResponseModel>> GetAllMealInCurrentSession(GetAllMealRequest pagingParams)
        //{
        //    var selectExpression = GetAllMealInCurrentSessionResponseModel.FromSession();
        //    var includes = new Expression<Func<Session, object>>[]
        //    {
        //        x => x.MealSessions,
        //        x => x.User.Kitchens
        //    };
        //    Expression<Func<Session, bool>> conditionAddition = e => e.StartTime < (pagingParams.SessionStartTime ?? DateTime.Now);

        //    var result = await _sessionRepository.GetWithPaging(pagingParams, conditionAddition, selectExpression, includes);

        //    return result;
        //}

        public async Task ChangeStatusSession(int sessionid)
        {
            var result = await _context.Sessions.FindAsync(sessionid);

            if (result != null && result.Status == true)
            {
                result.Status = false;
                await _transactionService.SaveTotalPriceAfterFinishSession(sessionid);

                var areas = await _context.SessionAreas.Where(a => a.SessionId == sessionid).Select(a => a.AreaId).ToListAsync();
                var areaIds = areas.Where(a => a.HasValue).Select(a => a.Value).ToList();

                var sessionR = new SessionRequestModel
                    {
                        SessionType = result.SessionType,
                        AreaIds = areaIds,
                    };
                   await CreateSessionForNextDay(sessionR);
                
                await _context.SaveChangesAsync();
            }
            else result.Status = true;

            await _context.SaveChangesAsync();
        }

        public async Task<List<SessionResponseModel>> GetAllSession()
        {
            var result = _context.Sessions.Include(x => x.SessionAreas).Select(x => new SessionResponseModel
            {
                SessionId = x.SessionId,
                CreateDate = ((DateTime)x.CreateDate).ToString("dd-MM-yyyy"),
                StartTime = ((DateTime)x.StartTime).ToString("HH:mm"),
                EndTime = ((DateTime)x.EndTime).ToString("HH:mm"),
                EndDate = ((DateTime)x.EndDate).ToString("dd-MM-yyyy"),
                UserId = x.UserId,
                SessionType = x.SessionType,
                SessionName = x.SessionName,
                AreaId = x.SessionAreas.FirstOrDefault().Area.AreaId,
                Status = x.Status,
                Message = "Success",
            });

            var mapped = result.Select(r => _mapper.Map<SessionResponseModel>(r)).ToList();
            return mapped;
        }

        public async Task<List<GetAllSessionByAreaIdResponseModel>> GetAllSessionByAreaId(int areaid)
        {

            var listSessionIds = await _context.SessionAreas.Where(x => x.AreaId == areaid).Select(x => x.SessionId).ToListAsync();
            var sessions = await _context.Sessions.Include(s => s.SessionAreas).ThenInclude(s => s.Area).Where(s => listSessionIds.Contains(s.SessionId)).Select(s => new GetAllSessionByAreaIdResponseModel
            {
                SessionId = s.SessionId,
                CreateDate = ((DateTime)s.CreateDate).ToString("dd-MM-yyyy"),
                StartTime = ((DateTime)s.StartTime).ToString("HH:mm"),
                EndTime = ((DateTime)s.EndTime).ToString("HH:mm"),
                EndDate = ((DateTime)s.EndDate).ToString("dd-MM-yyyy"),
                UserId = s.UserId,
                SessionType = s.SessionType,
                SessionName = s.SessionName,
                AreaDto = new AreaDto
                {
                    AreaId = areaid,
                    Address = s.SessionAreas.FirstOrDefault(sa => sa.AreaId == areaid).Area.Address,
                    DistrictId = s.SessionAreas.FirstOrDefault(sa => sa.AreaId == areaid).Area.DistrictId,
                },
                Status = s.Status,
            }).ToListAsync();

            var mappedResults = sessions.Select(s => _mapper.Map<GetAllSessionByAreaIdResponseModel>(s)).ToList();
            return mappedResults;
        }

        public Task<List<GetAllSessionByAreaIdResponseModel>> GetAllSessionByAreaIdWithStatusTrue(int areaid)
        {

            var result = _context.SessionAreas.Include(x => x.Area).Include(x => x.Session).Where(x => x.AreaId == areaid && x.Session.Status == true).Select(x => new GetAllSessionByAreaIdResponseModel
            {
                SessionId = x.Session.SessionId,
                CreateDate = ((DateTime)x.Session.CreateDate).ToString("dd-MM-yyyy"),
                StartTime = ((DateTime)x.Session.StartTime).ToString("HH:mm"),
                EndTime = ((DateTime)x.Session.EndTime).ToString("HH:mm"),
                EndDate = ((DateTime)x.Session.EndDate).ToString("dd-MM-yyyy"),
                UserId = x.Session.UserId,
                SessionType = x.Session.SessionType,
                SessionName = x.Session.SessionName,
                AreaDto = new AreaDto
                {
                    AreaId = areaid,
                    Address = x.Session.SessionAreas.FirstOrDefault(sa => sa.AreaId == areaid).Area.Address,
                    DistrictId = x.Session.SessionAreas.FirstOrDefault(sa => sa.AreaId == areaid).Area.DistrictId,
                },
                Status = x.Session.Status,
            });

            var mappedResults = result.Select(session => _mapper.Map<GetAllSessionByAreaIdResponseModel>(session)).ToList();
            return Task.FromResult(mappedResults);
        }

        //public Task DeleteSession(int sessionId)
        //{
        //    var result = _sessionRepository.Delete(sessionId);
        //    return result;
        //}

        public async Task<GetSingleSessionBySessionIdResponseModel> GetSingleSessionBySessionId(int sessionid)
        {
            var result = _context.Sessions.Include(x => x.SessionAreas).ThenInclude(x => x.Area).Where(x => x.SessionId == sessionid).Select(x => new GetSingleSessionBySessionIdResponseModel
            {
                SessionId = x.SessionId,
                CreateDate = ((DateTime)x.CreateDate).ToString("dd-MM-yyyy"),
                StartTime = ((DateTime)x.StartTime).ToString("HH:mm"),
                EndTime = ((DateTime)x.EndTime).ToString("HH:mm"),
                EndDate = ((DateTime)x.EndDate).ToString("dd-MM-yyyy"),
                Status = x.Status,
                SessionType = x.SessionType,
                SessionName = x.SessionName,
                UserDtoGetSingleSessionBySessionId = new UserDtoGetSingleSessionBySessionId
                {
                    UserId = x.User.UserId,
                    Username = x.User.Username,
                    Address = x.User.Address,
                    DistrictId = x.User.DistrictId,
                    Email = x.User.Email,
                    Name = x.User.Name,
                    Phone = x.User.Phone,
                },
                AreaDtoGetSingleSessionBySessionId = new AreaDtoGetSingleSessionBySessionId
                {
                    AreaId = x.SessionAreas.FirstOrDefault(sa => sa.SessionId == sessionid).Area.AreaId,
                    Address = x.SessionAreas.FirstOrDefault(sa => sa.SessionId == sessionid).Area.Address,
                    AreaName = x.SessionAreas.FirstOrDefault(sa => sa.SessionId == sessionid).Area.AreaName,
                },
            }).FirstOrDefault();

            var mapped = _mapper.Map<GetSingleSessionBySessionIdResponseModel>(result);
            return mapped;
        }

        public async Task<SessionResponseModel> CreateSessionForNextDay(SessionRequestModel sessionRequest)
        {
            var responseModel = new SessionResponseModel(); // Initialize the response model

            try
            {
                var entity = _mapper.Map<Session>(sessionRequest);
                var sessionTypeLower = entity.SessionType.ToLower();


                if (sessionRequest.AreaIds != null)
                {
                        SetSessionProperties(entity, sessionTypeLower, sessionRequest.AreaIds);
                        entity.EndDate = GetDateTimeTimeZoneVietNam().AddDays(1);
                        entity.SessionName = $"Session: {entity.SessionType} , In: {((DateTime)entity.EndDate).ToString("dd-MM-yyyy")}";
                        
                        var result = await _sessionRepository.Create(entity, true);

                        if (result != null && sessionRequest.AreaIds != null)
                        {
                            var uniqueAreaIds = new HashSet<int>();

                            foreach (var areaId in sessionRequest.AreaIds)
                            {
                                if (uniqueAreaIds.Add(areaId))
                                {
                                    var sessionArea = new SessionArea
                                    {
                                        SessionId = result.SessionId,
                                        AreaId = areaId,
                                    };
                                    await _context.AddAsync(sessionArea);
                                }
                                await _context.SaveChangesAsync();
                            }

                        }
                        responseModel = _mapper.Map<SessionResponseModel>(result);

                        responseModel.StartTime = result.StartTime?.ToString("HH:mm");
                        responseModel.EndTime = result.EndTime?.ToString("HH:mm");
                        responseModel.CreateDate = result.CreateDate?.ToString("dd-MM-yyyy");
                        responseModel.EndDate = result.EndDate?.AddDays(1).ToString("dd-MM-yyyy");

                        responseModel.Message = "Success";

                    
                }
                else
                {
                    responseModel.Message = "Error: Not Exist Area to Create Session";
                }

                return responseModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in CreateSession: {Message}", ex.Message);

                responseModel.Message = "Error: " + ex.Message;
                return responseModel;
            }
        }

        //public async Task<List<GetAllSessionByAreaIdResponseModel>> GetAllSessionByAreaIdWithStatusTrueInDay(int areaid)
        //{
        //    var datenow = GetDateTimeTimeZoneVietNam();
        //    var result = _context.SessionAreas.Include(x => x.Session).Include(x => x.Area).Where(x => x.AreaId == areaid && x.Session.Status == "ON" && x.Session.CreateDate.Value.Date == datenow).Select(x => new GetAllSessionByAreaIdResponseModel
        //    {
        //        SessionId = x.Session.SessionId,
        //        CreateDate = ((DateTime)x.Session.CreateDate).ToString("dd-MM-yyyy"),
        //        StartTime = ((DateTime)x.Session.StartTime).ToString("HH:mm"),
        //        EndTime = ((DateTime)x.Session.EndTime).ToString("HH:mm"),
        //        EndDate = ((DateTime)x.Session.EndDate).ToString("dd-MM-yyyy"),
        //        UserId = x.Session.UserId,
        //        SessionType = x.Session.SessionType,
        //        SessionName = x.Session.SessionName,
        //        AreaDto = new AreaDto
        //        {
        //            AreaId = x.Session.SessionAreas.FirstOrDefault(sa => sa.AreaId == areaid).Area.AreaId,
        //            Address = x.Session.SessionAreas.FirstOrDefault(sa => sa.AreaId == areaid).Area.Address,
        //            DistrictId = x.Session.SessionAreas.FirstOrDefault(sa => sa.AreaId == areaid).Area.DistrictId,
        //        },
        //        Status = x.Session.Status,
        //    }).ToList();

        //    var mappedResults = result.Select(session => _mapper.Map<GetAllSessionByAreaIdResponseModel>(session)).ToList();
        //    return mappedResults;
        //}
    }
}
