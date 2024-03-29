﻿using AutoMapper;
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
        private readonly ISessionAreaService _sessionAreaService;
        private readonly HomeMealTasteContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<SessionService> _logger;
        public SessionService(HomeMealTasteContext context, ISessionRepository sessionRepository, IMapper mapper, ITransactionService transactionService, ILogger<SessionService> logger, ISessionAreaService sessionAreaService)
        {
            _context = context;
            _sessionRepository = sessionRepository;
            _mapper = mapper;
            _transactionService = transactionService;
            _logger = logger;
            _sessionAreaService = sessionAreaService;
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
        private async Task<bool> SessionTypeExistsInAreaInDayNow(List<int> areaId, string sessionType, DateTime date)
        {
            //var date = GetDateTimeTimeZoneVietNam();

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

            //foreach (var areaid in areaId)
            //{
            //    var sessionIds = _context.SessionAreas
            //        .Where(sa => sa.AreaId == areaId)
            //        .Select(sa => sa.SessionId);

            //    var sessionTypeExists = _context.Sessions
            //        .Any(s => sessionIds.Contains(s.SessionId) && s.EndDate == date && s.SessionType.ToLower() == sessionType.ToLower());

            //    if (sessionTypeExists)
            //    {
            //        return false;
            //    }
            //}

            //return true;

        }

        private async Task<bool> SessionTypeExistsInAreaInNextDay(List<int> areaId, string sessionType, DateTime date)
        {
            //var date = GetDateTimeTimeZoneVietNam().AddDays(1);

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
        //public async Task<SessionResponseModel> CreateSession(SessionRequestModel sessionRequest)
        //{

        //    var responseModel = new SessionResponseModel(); // Initialize the response model

        //    try
        //    {
        //        var entity = _mapper.Map<Session>(sessionRequest);
        //        var sessionTypeLower = entity.SessionType.ToLower();


        //        if (sessionRequest.AreaIds != null)
        //        {
        //            if (await SessionTypeExistsInAreaInDayNow(sessionRequest.AreaIds, sessionTypeLower))
        //            {
        //                SetSessionProperties(entity, sessionTypeLower, sessionRequest.AreaIds);
        //                entity.EndDate = GetDateTimeTimeZoneVietNam();
        //                entity.SessionName = $"Session: {entity.SessionType} , In: {((DateTime)entity.CreateDate).ToString("dd-MM-yyyy")}";

        //                var result = await _sessionRepository.Create(entity, true);
        //                if (result != null && sessionRequest.AreaIds != null)
        //                {
        //                    var uniqueAreaIds = new HashSet<int>();

        //                    foreach (var areaId in sessionRequest.AreaIds)
        //                    {
        //                        if (uniqueAreaIds.Add(areaId))
        //                        {
        //                            var sessionArea = new SessionArea
        //                            {
        //                                SessionId = result.SessionId,
        //                                AreaId = areaId,
        //                            };
        //                            await _context.AddAsync(sessionArea);
        //                        }
        //                        await _context.SaveChangesAsync();
        //                    }

        //                }
        //                responseModel = _mapper.Map<SessionResponseModel>(result);

        //                responseModel.StartTime = result.StartTime?.ToString("HH:mm");
        //                responseModel.EndTime = result.EndTime?.ToString("HH:mm");
        //                responseModel.CreateDate = result.CreateDate?.ToString("dd-MM-yyyy");
        //                responseModel.EndDate = result.EndDate?.ToString("dd-MM-yyyy");

        //                responseModel.Message = "Success";
        //            }
        //            else
        //            {
        //                responseModel.Message = "Error: sessionType is EXISTED";
        //            }
        //        }
        //        else
        //        {
        //            responseModel.Message = "Error: Not Exist Area to Create Session";
        //        }

        //        return responseModel;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred in CreateSession: {Message}", ex.Message);

        //        responseModel.Message = "Error: " + ex.Message;
        //        return responseModel;
        //    }
        //}

        private void SetSessionProperties(Session entity, string sessionTypeLower, List<int> areaId)
        {
            //entity.CreateDate = GetDateTimeTimeZoneVietNam();

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

            //entity.Status = "OPEN";
            entity.UserId = 2;
        }
        public async Task<List<SessionResponseModel>> CreateSessionWithDay(SessionRequestModel sessionRequest)
        {

            var responseModel = new SessionResponseModel();
            var resultList = new List<SessionResponseModel>();
            var currentDate = DateTime.UtcNow;
            var sessionDate = DateTime.ParseExact(sessionRequest.Date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            if (sessionDate.Date < currentDate.Date)
            {

                throw new Exception("Cannot create a session with a date in the past.");
            }
            foreach (var sessionTypeItem in sessionRequest.SessionType)
            {
                var entity = new Session();
                var sessionTypeLower = sessionTypeItem.ToString().ToLower();
                entity.CreateDate = GetDateTimeTimeZoneVietNam();
                entity.EndDate = DateTime.ParseExact(sessionRequest.Date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                entity.Status = (sessionRequest.Status?.ToString().ToLower() ?? "OPEN").ToUpperInvariant();
                if (entity.Status == null)
                {
                    entity.Status = "OPEN";
                }
                if (sessionRequest.AreaIds != null)
                {
                    if (await SessionTypeExistsInAreaInDayNow(sessionRequest.AreaIds, sessionTypeLower, (DateTime)entity.EndDate))
                    {
                        SetSessionProperties(entity, sessionTypeLower, sessionRequest.AreaIds);
                        //entity.EndDate = GetDateTimeTimeZoneVietNam();
                        if (DateTime.TryParseExact(sessionRequest.Date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                        {
                            entity.SessionName = $"Session: {entity.SessionType} , In: {parsedDate.ToString("dd-MM-yyyy")}";

                        }

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
                                        Status = "OPEN",
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

                        resultList.Add(responseModel);
                        //responseModel.Message = "Success";
                    }
                    else throw new Exception("Session Type is Existed");
                }
            }
            return resultList;

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

        public async Task ChangeStatusSession(ChangeStatusSessionRequestModel request)
        {
            var datenow = GetDateTimeTimeZoneVietNam();

            if (request.sessionId != null)
            {

                var resultSession = await _context.Sessions.FindAsync(request.sessionId);
                var resultSessionArea = _context.SessionAreas.Where(x => x.SessionId == request.sessionId).ToList();

                if (resultSession != null)
                {
                    if (request.status.Equals("OPEN" ,StringComparison.OrdinalIgnoreCase))
                    {
                        if (resultSession.Status.Equals("BOOKING", StringComparison.OrdinalIgnoreCase) && resultSession.EndDate.Value.Date >= datenow.Date)
                        {
                            resultSession.Status = "OPEN";
                        }
                        else
                        {
                            throw new Exception($"Can not change status to OPEN because status of Session {resultSession.SessionId} is not Booking");
                        }
                    }
                    else if (request.status.Equals("BOOKING", StringComparison.OrdinalIgnoreCase))
                    {
                        var check = false;

                        if (resultSession.Status.Equals("OPEN", StringComparison.OrdinalIgnoreCase) && resultSession.EndDate.Value.Date >= datenow.Date)
                        {
                            foreach (var sessionArea in resultSessionArea)
                            {
                                var mealSession = _context.MealSessions.Where(x => x.SessionId == sessionArea.SessionId && x.AreaId == sessionArea.AreaId).ToList();
                                if(mealSession.Count > 0)
                                {
                                    foreach (var mealSessionItem in mealSession)
                                    {
                                        if (!mealSessionItem.Status.Equals("PROCESSING"))
                                        {
                                            check = true;
                                        }
                                        else
                                        {
                                            check = false;
                                            throw new Exception($"Can not change status to BOOKING because status of MealSession {mealSessionItem.MealSessionId} is Processing");
                                        }
                                    }
                                }
                                else
                                {
                                    check = true;
                                }
                            }
                        }
                        else
                        {
                            check = false;
                            throw new Exception($"Can not change status to BOOKING because status of Session {resultSession.SessionId} is not Open");
                        }
                        if (check)
                        {
                            resultSession.Status = "BOOKING";
                            _context.Sessions.Update(resultSession);
                        }
                    }
                    else if (request.status.Equals("ONGOING", StringComparison.OrdinalIgnoreCase))
                    {
                        var check = false;

                        if (resultSession.Status.Equals("BOOKING", StringComparison.OrdinalIgnoreCase) && resultSession.EndDate.Value.Date >= datenow.Date)
                        {
                            foreach (var sessionArea in resultSessionArea)
                            {
                                var mealSession = _context.MealSessions.Where(x => x.SessionId == sessionArea.SessionId && x.AreaId == sessionArea.AreaId).ToList();
                                if(mealSession.Count > 0)
                                {
                                    foreach (var mealSessionItem in mealSession)
                                    {
                                        if (mealSessionItem.Status.Equals("REJECTED") || mealSessionItem.Status.Equals("CANCELLED") || mealSessionItem.Status.Equals("COMPLETED"))
                                        {
                                            check = true;
                                        }
                                        else
                                        {
                                            check = false;
                                            throw new Exception($"Can not change status to BOOKING because status of MealSession {mealSessionItem.MealSessionId} is {mealSessionItem.Status}");
                                        }
                                    }
                                }
                                else
                                {
                                    check = true;
                                }
                                
                            }
                        }
                        else
                        {
                            throw new Exception($"Can not change status to ONGOING because status of Session {resultSession.SessionId} is not Booking");
                        }
                        if (check)
                        {
                            resultSession.Status = "ONGOING";
                            _context.Sessions.Update(resultSession);
                        }
                    }
                    else if (request.status.Equals("CLOSED", StringComparison.OrdinalIgnoreCase))
                    {
                        bool check = false;
                        if (resultSession.Status.Equals("ONGOING", StringComparison.OrdinalIgnoreCase) && resultSession.EndDate.Value.Date >= datenow.Date)
                        {
                            foreach (var sessionAreas in resultSessionArea)
                            {
                                if (sessionAreas.Status.Equals("FINISHED") || sessionAreas.Status.Equals("CANCELLED"))
                                {
                                    check = true;
                                }
                                else
                                {
                                    check = false;
                                    throw new Exception($"Can not change status to CLOSED because SESSION AREA {sessionAreas.SessionAreaId} is not final state");
                                }
                            }
                        }
                        else
                        {
                            check = false;
                            throw new Exception($"Can not change status to CLOSED because status of Session {resultSession.SessionId} is not ONGOING");
                        }
                        if (check)
                        {
                            resultSession.Status = "CLOSED";
                            //chuyen tien cho chef tu vi admin o day
                            await _transactionService.TransferTotalPriceToChefAfterClosedSession(resultSession.SessionId);
                            _context.Sessions.Update(resultSession);
                        }

                    }
                    else if (request.status.Equals("CANCELLED"))
                    {
                        bool check = false;

                        if (resultSession.Status.Equals("OPEN", StringComparison.OrdinalIgnoreCase) && resultSession.EndDate.Value.Date >= datenow.Date)
                        {
                            foreach (var sessionAreas in resultSessionArea)
                            {
                                if (sessionAreas.Status.Equals("CANCELLED"))
                                {
                                    check = true;
                                }
                                else
                                {
                                    check = false;
                                    throw new Exception($"Can not change status to CANCELLED because SESSION AREA {sessionAreas.SessionAreaId} is not CANCELLED");
                                }
                            }
                        }
                        else
                        {
                            check = false;
                            throw new Exception($"Can not change status to CANCELLED because SESSION {resultSession.SessionId} is not OPEN");
                        }
                        if (check)
                        {
                            resultSession.Status = "CANCELLED";
                            _context.Sessions.Update(resultSession);
                        }
                    }
                }
                else
                {
                    throw new Exception("Can not find Session");
                }

                await _context.SaveChangesAsync();

            }
        }

        public async Task<List<GetAllSessionResponseModel>> GetAllSession()
        {
            var result = _context.Sessions.Include(x => x.SessionAreas).ThenInclude(a => a.Area).Select(x => new GetAllSessionResponseModel
            {
                SessionId = x.SessionId,
                CreateDate = ((DateTime)x.CreateDate).ToString("dd-MM-yyyy"),
                StartTime = ((DateTime)x.StartTime).ToString("HH:mm"),
                EndTime = ((DateTime)x.EndTime).ToString("HH:mm"),
                EndDate = ((DateTime)x.EndDate).ToString("dd-MM-yyyy"),
                UserId = x.UserId,
                AreaDtoGetAllSession = x.SessionAreas.Select(a => new AreaDtoGetAllSession
                {
                    AreaId = a.Area.AreaId,
                    AreaName = a.Area.AreaName,
                    Address = a.Area.Address,
                }).ToList(),
                SessionType = x.SessionType,
                SessionName = x.SessionName,

                Status = x.Status,
                Message = "Success",
            });

            var mapped = result.Select(r => _mapper.Map<GetAllSessionResponseModel>(r)).ToList();
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
                AreaDto = s.SessionAreas.Select(s => new AreaDto
                {
                    AreaId = s.Area.AreaId,
                    Address = s.Area.Address,
                    DistrictId = s.Area.DistrictId,
                }).ToList(),
                Status = s.Status,
            }).ToListAsync();

            var mappedResults = sessions.Select(s => _mapper.Map<GetAllSessionByAreaIdResponseModel>(s)).ToList();
            return mappedResults;
        }

        public Task<List<GetAllSessionByAreaIdResponseModel>> GetAllSessionByAreaIdWithStatusOpen(int areaid)
        {

            var result = _context.SessionAreas.Include(x => x.Area).Include(x => x.Session).Where(x => x.AreaId == areaid && x.Session.Status.Equals("OPEN")).Select(x => new GetAllSessionByAreaIdResponseModel
            {
                SessionId = x.Session.SessionId,
                CreateDate = ((DateTime)x.Session.CreateDate).ToString("dd-MM-yyyy"),
                StartTime = ((DateTime)x.Session.StartTime).ToString("HH:mm"),
                EndTime = ((DateTime)x.Session.EndTime).ToString("HH:mm"),
                EndDate = ((DateTime)x.Session.EndDate).ToString("dd-MM-yyyy"),
                UserId = x.Session.UserId,
                SessionType = x.Session.SessionType,
                SessionName = x.Session.SessionName,
                AreaDto = x.Session.SessionAreas.Select(s => new AreaDto
                {
                    AreaId = s.Area.AreaId,
                    Address = s.Area.Address,
                    DistrictId = s.Area.DistrictId,
                }).ToList(),
                Status = x.Session.Status,
            });

            var mappedResults = result.Select(session => _mapper.Map<GetAllSessionByAreaIdResponseModel>(session)).ToList();
            return Task.FromResult(mappedResults);
        }

        public async Task<GetSingleSessionBySessionIdResponseModel> GetSingleSessionBySessionId(int sessionid)
        {
            var result = _context.Sessions.Include(x => x.SessionAreas).ThenInclude(a => a.Area).Where(x => x.SessionId == sessionid).Select(x => new GetSingleSessionBySessionIdResponseModel
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
                AreaDtoGetSingleSessionBySessionId = x.SessionAreas.Select(a => new AreaDtoGetSingleSessionBySessionId
                {
                    AreaId = a.Area.AreaId,
                    AreaName = a.Area.AreaName,
                    Address = a.Area.Address,
                }).ToList()
            }).FirstOrDefault();

            var mapped = _mapper.Map<GetSingleSessionBySessionIdResponseModel>(result);
            return mapped;
        }

        public async Task<SessionResponseModel> CreateSessionForNextDay(SessionForChangeStatusRequestModel sessionRequest)
        {
            var responseModel = new SessionResponseModel(); // Initialize the response model


            var entity = _mapper.Map<Session>(sessionRequest);
            var sessionTypeLower = entity.SessionType.ToLower();

            //string createDateString = sessionRequest.CreateDate.ToString();
            //entity.CreateDate = DateTime.ParseExact(createDateString, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            //string endDateString = sessionRequest.EndDate.ToString(); 
            //entity.EndDate = DateTime.ParseExact(endDateString, "dd-MM-yyyy", CultureInfo.InvariantCulture).AddDays(1);

            entity.CreateDate = sessionRequest.CreateDate;
            entity.EndDate = sessionRequest.EndDate.Value.AddDays(1);
            if (sessionRequest.AreaIds != null)
            {
                var check = await SessionTypeExistsInAreaInNextDay(sessionRequest.AreaIds, sessionTypeLower, (DateTime)entity.EndDate);
                if (check)
                {
                    SetSessionProperties(entity, sessionTypeLower, sessionRequest.AreaIds);
                    //entity.EndDate = GetDateTimeTimeZoneVietNam().AddDays(1);
                    //if (DateTime.TryParseExact(endDateString, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                    //{
                    //    entity.SessionName = $"Session: {entity.SessionType} , In: {parsedDate.AddDays(1).ToString("dd-MM-yyyy")}";

                    //}
                    entity.SessionName = $"Session: {entity.SessionType} , In: {((DateTime)entity.EndDate).ToString("dd-MM-yyyy")}";
                    entity.Status = "OPEN";
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

                    //responseModel.Message = "Success";
                }
                else
                {
                    //responseModel.Message = "Error: sessionType is EXISTED";
                }

            }
            else
            {
                //responseModel.Message = "Error: Not Exist Area to Create Session";
            }

            return responseModel;

        }


        public async Task<UpdateSessionAndAreaInSessionResponseModel> UpdateSessionAndAreaInSession(UpdateSessionAndAreaInSessionRequestModel request)
        {
            var responseModel = new UpdateSessionAndAreaInSessionResponseModel();
            var datenow = GetDateTimeTimeZoneVietNam();
            var sessionId = _context.SessionAreas.Where(x => x.SessionId == request.SessionId).Select(x => x.SessionId).FirstOrDefault();
            var resultSession = _context.Sessions.Where(x => x.SessionId == sessionId && x.EndDate.Value.Date >= datenow.Date && x.Status.Equals("OPEN")).FirstOrDefault();
            if (resultSession != null)
            {
                //find list areaIds among with sessionId inputed and update
                var listAreaId = _context.SessionAreas.Where(x => x.SessionId == sessionId).ToList();
                if (listAreaId.Count > 0)
                {
                    // Remove existing SessionAreas
                    _context.SessionAreas.RemoveRange(listAreaId);
                    await _context.SaveChangesAsync();

                    // Add new SessionAreas
                    for (int i = 0; i < request.AreaIds.Count; i++)
                    {
                        var newSessionArea = new SessionArea
                        {
                            SessionId = sessionId,
                            AreaId = request.AreaIds[i],
                            Status = "OPEN",
                        };
                        _context.SessionAreas.Add(newSessionArea);
                    }
                    await _context.SaveChangesAsync();
                }
            }
            else throw new Exception("Can not Update Session in the past");
            responseModel = _mapper.Map<UpdateSessionAndAreaInSessionResponseModel>(resultSession);
            responseModel.StartTime = resultSession.StartTime?.ToString("HH:mm");
            responseModel.EndTime = resultSession.EndTime?.ToString("HH:mm");
            responseModel.CreateDate = resultSession.CreateDate?.ToString("dd-MM-yyyy");
            responseModel.EndDate = resultSession.EndDate?.ToString("dd-MM-yyyy");

            return responseModel;
        }

        public async Task<List<GetAllSessionResponseModel>> GetAllSessionStatusBooking()
        {
            var result = _context.Sessions.Include(x => x.SessionAreas).ThenInclude(a => a.Area)
                .Where(x => x.Status.Equals("BOOKING")).Select(x => new GetAllSessionResponseModel
                {
                    SessionId = x.SessionId,
                    CreateDate = ((DateTime)x.CreateDate).ToString("dd-MM-yyyy"),
                    StartTime = ((DateTime)x.StartTime).ToString("HH:mm"),
                    EndTime = ((DateTime)x.EndTime).ToString("HH:mm"),
                    EndDate = ((DateTime)x.EndDate).ToString("dd-MM-yyyy"),
                    UserId = x.UserId,
                    AreaDtoGetAllSession = x.SessionAreas.Select(a => new AreaDtoGetAllSession
                    {
                        AreaId = a.Area.AreaId,
                        AreaName = a.Area.AreaName,
                        Address = a.Area.Address,
                    }).ToList(),
                    SessionType = x.SessionType,
                    SessionName = x.SessionName,

                    Status = x.Status,
                    Message = "Success",
                });

            var mapped = result.Select(r => _mapper.Map<GetAllSessionResponseModel>(r)).ToList();
            return mapped;
        }

        public async Task<List<SessionResponseModel>> GetAllSessionWithStatusTrueAndBookingSlotTrue()
        {
            var result = _context.Sessions.Include(x => x.SessionAreas).ThenInclude(x => x.Area).Where(x => x.Status.Equals("ONGOING")).Select(x => new SessionResponseModel
            {
                SessionId = x.SessionId,
                CreateDate = ((DateTime)x.CreateDate).ToString("dd-MM-yyyy"),
                StartTime = ((DateTime)x.StartTime).ToString("HH:mm"),
                EndTime = ((DateTime)x.EndTime).ToString("HH:mm"),
                EndDate = ((DateTime)x.EndDate).ToString("dd-MM-yyyy"),
                Status = x.Status,
                SessionName = x.SessionName,
                SessionType = x.SessionType,
                UserId = x.UserId,
                AreaId = x.SessionAreas.FirstOrDefault().Area.AreaId,
            }).ToList();

            var mapped = result.Select(r => _mapper.Map<SessionResponseModel>(r)).ToList();
            return mapped;
        }

        public async Task DeleteSession(int sessionId)
        {
            var sessionIdd = _context.Sessions.Include(x => x.SessionAreas).Where(x => x.SessionId == sessionId).FirstOrDefault();
            if (sessionIdd != null)
            {
                _context.Sessions.Remove(sessionIdd);

                var listSessionArea = _context.SessionAreas.Where(x => x.SessionId == sessionIdd.SessionId).ToList();
                if (listSessionArea.Count > 0)
                {
                    foreach (var area in listSessionArea)
                    {
                        _context.SessionAreas.Remove(area);
                    }
                }
            }
            else
            {
                throw new Exception("Session have Meal can not delete!");
            }
            _context.SaveChanges();
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
