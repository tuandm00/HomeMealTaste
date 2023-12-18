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

namespace HomeMealTaste.Services.Implement
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly ITransactionService _transactionService;
        private readonly HomeMealTasteContext _context;
        private readonly IMapper _mapper;

        public SessionService(HomeMealTasteContext context, ISessionRepository sessionRepository, IMapper mapper, ITransactionService transactionService)
        {
            _context = context;
            _sessionRepository = sessionRepository;
            _mapper = mapper;
            _transactionService = transactionService;
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
        private async Task<bool> SessionTypeExistsInAreaInDayNow(int areaId, string sessionType)
        {
            var date = GetDateTimeTimeZoneVietNam();
            var sessiontype = _context.Sessions.Where(x => x.AreaId == areaId && x.SessionType == sessionType && x.CreateDate == date).Select(x => x.SessionType).ToList();
            
            foreach(var type in sessiontype)
            {
                if ((type.ToLower()).Equals(sessionType))
                {
                    return false;
                }
            }
            return true;
            
        }
        public async Task<SessionResponseModel> CreateSession(SessionRequestModel sessionRequest)
        {
            var entity = _mapper.Map<Session>(sessionRequest);
            var sessionTypeLower = entity.SessionType.ToLower();
            var sessionName = sessionRequest.SessionName;
            var areaId = _context.Sessions.Where(x => x.AreaId == entity.AreaId).Select(x => x.AreaId).FirstOrDefault();
            if (areaId != null)
            {

                if (await SessionTypeExistsInAreaInDayNow((int)areaId, sessionTypeLower))
                {
                    if (string.Equals(sessionTypeLower, "lunch", StringComparison.OrdinalIgnoreCase))
                    {
                        entity.CreateDate = GetDateTimeTimeZoneVietNam();
                        entity.StartTime = GetDateTimeTimeZoneVietNam().Date.AddHours(10);
                        entity.EndTime = entity.StartTime.Value.AddHours(2);
                        entity.EndDate = GetDateTimeTimeZoneVietNam();
                        entity.Status = true;
                        entity.UserId = 1;
                        entity.SessionType = "Lunch";
                        entity.AreaId = areaId;
                        entity.SessionName = sessionName;

                    }
                    else if (string.Equals(sessionTypeLower, "evening", StringComparison.OrdinalIgnoreCase))
                    {
                        entity.CreateDate = GetDateTimeTimeZoneVietNam();
                        entity.StartTime = GetDateTimeTimeZoneVietNam().Date.AddHours(16);
                        entity.EndTime = entity.StartTime.Value.AddHours(4);
                        entity.EndDate = GetDateTimeTimeZoneVietNam();
                        entity.Status = true;
                        entity.UserId = 1;
                        entity.SessionType = "Evening";
                        entity.AreaId = areaId;
                        entity.SessionName = sessionName;


                    }
                    else if (string.Equals(sessionTypeLower, "dinner", StringComparison.OrdinalIgnoreCase))
                    {
                        entity.CreateDate = GetDateTimeTimeZoneVietNam();
                        entity.StartTime = GetDateTimeTimeZoneVietNam().Date.AddHours(17);
                        entity.EndTime = entity.StartTime.Value.AddHours(2);
                        entity.EndDate = GetDateTimeTimeZoneVietNam();
                        entity.Status = true;
                        entity.UserId = 1;
                        entity.SessionType = "Dinner";
                        entity.AreaId = areaId;
                        entity.SessionName = sessionName;

                    }
                }
                else
                {
                    throw new Exception("sessionType is EXISTED");
                }
            }
            else
            {
                throw new Exception("Not Exist Area to Create Session");
            }
            var result = await _sessionRepository.Create(entity, true);

            var responseModel = _mapper.Map<SessionResponseModel>(result);

            responseModel.StartTime = result.StartTime?.ToString("HH:mm");
            responseModel.EndTime = result.EndTime?.ToString("HH:mm");
            responseModel.CreateDate = result.CreateDate?.ToString("dd-MM-yyyy");
            responseModel.EndDate = result.EndDate?.ToString("dd-MM-yyyy");

            return responseModel;
        }

        public async Task<SessionResponseModel> UpdateEndTime(int sessionId, DateTime endTime)
        {
            var entity = await _sessionRepository.GetFirstOrDefault(x => x.SessionId == sessionId);
            entity.EndTime = endTime;

            var response = await _sessionRepository.Update(entity);

            return _mapper.Map<SessionResponseModel>(response);

        }

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
            }
            else result.Status = true;

            await _context.SaveChangesAsync();
        }

        public async Task<List<SessionResponseModel>> GetAllSession()
        {
            var result = _context.Sessions.ToList();
            var mapped = result.Select(session =>
            {
                var responseModel = _mapper.Map<SessionResponseModel>(session);

                responseModel.CreateDate = session.CreateDate?.ToString("dd-MM-yyyy");
                responseModel.EndDate = session.EndDate?.ToString("dd-MM-yyyy");
                responseModel.StartTime = session.StartTime?.ToString("HH:mm");
                responseModel.EndTime = session.EndTime?.ToString("HH:mm");

                return responseModel;
            }).ToList();

            return mapped;
        }

        public Task<List<GetAllSessionByAreaIdResponseModel>> GetAllSessionByAreaId(int areaid)
        {

            var result = _context.Sessions.Where(x => x.AreaId == areaid).Select(x => new GetAllSessionByAreaIdResponseModel
            {
                SessionId = x.SessionId,
                CreateDate = ((DateTime)x.CreateDate).ToString("dd-MM-yyyy"),
                StartTime = ((DateTime)x.StartTime).ToString("HH:mm"),
                EndTime = ((DateTime)x.EndTime).ToString("HH:mm"),
                EndDate = ((DateTime)x.EndDate).ToString("dd-MM-yyyy"),
                UserId = x.UserId,
                SessionType = x.SessionType,
                SessionName = x.SessionName,
                AreaDto = new AreaDto
                {
                    AreaId = areaid,
                    Address = x.Area.Address,
                    DistrictId = x.Area.DistrictId,
                },
                Status = x.Status,
            });

            var mappedResults = result.Select(session => _mapper.Map<GetAllSessionByAreaIdResponseModel>(session)).ToList();
            return Task.FromResult(mappedResults);
        }
        
        public Task<List<GetAllSessionByAreaIdResponseModel>> GetAllSessionByAreaIdWithStatusTrue(int areaid)
        {

            var result = _context.Sessions.Where(x => x.AreaId == areaid && x.Status == true).Select(x => new GetAllSessionByAreaIdResponseModel
            {
                SessionId = x.SessionId,
                CreateDate = ((DateTime)x.CreateDate).ToString("dd-MM-yyyy"),
                StartTime = ((DateTime)x.StartTime).ToString("HH:mm"),
                EndTime = ((DateTime)x.EndTime).ToString("HH:mm"),
                EndDate = ((DateTime)x.EndDate).ToString("dd-MM-yyyy"),
                UserId = x.UserId,
                SessionType = x.SessionType,
                SessionName= x.SessionName,
                AreaDto = new AreaDto
                {
                    AreaId = areaid,
                    Address = x.Area.Address,
                    DistrictId = x.Area.DistrictId,
                },
                Status = x.Status,
            });

            var mappedResults = result.Select(session => _mapper.Map<GetAllSessionByAreaIdResponseModel>(session)).ToList();
            return Task.FromResult(mappedResults);
        }

        public Task DeleteSession(int sessionId)
        {
            var result = _sessionRepository.Delete(sessionId);
            return result;
        }

        public async Task<GetSingleSessionBySessionIdResponseModel> GetSingleSessionBySessionId(int sessionid)
        {
            var result = _context.Sessions.Where(x => x.SessionId == sessionid).Select(x => new GetSingleSessionBySessionIdResponseModel
            {
                SessionId = x.SessionId,
                CreateDate = ((DateTime)x.CreateDate).ToString("dd-MM-yyyy"),
                StartTime = ((DateTime)x.StartTime).ToString("HH:mm"),
                EndTime = ((DateTime)x.EndTime).ToString("HH:mm"),
                EndDate = ((DateTime)x.EndDate).ToString("HH:mm"),
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
                    AreaId = x.Area.AreaId,
                    Address = x.Area.Address,
                    AreaName = x.Area.AreaName,
                },
            }).FirstOrDefault();

            return _mapper.Map<GetSingleSessionBySessionIdResponseModel>(result);
        }

        public async Task<List<GetAllSessionByAreaIdResponseModel>> GetAllSessionByAreaIdWithStatusTrueInDay(int areaid)
        {
            var datenow = GetDateTimeTimeZoneVietNam();
            var result = _context.Sessions.Where(x => x.AreaId == areaid && x.Status == true && x.CreateDate.Value.Date == datenow).Select(x => new GetAllSessionByAreaIdResponseModel
            {
                SessionId = x.SessionId,
                CreateDate = ((DateTime)x.CreateDate).ToString("dd-MM-yyyy"),
                StartTime = ((DateTime)x.StartTime).ToString("HH:mm"),
                EndTime = ((DateTime)x.EndTime).ToString("HH:mm"),
                EndDate = ((DateTime)x.EndDate).ToString("dd-MM-yyyy"),
                UserId = x.UserId,
                SessionType = x.SessionType,
                SessionName = x.SessionName,
                AreaDto = new AreaDto
                {
                    AreaId = areaid,
                    Address = x.Area.Address,
                    DistrictId = x.Area.DistrictId,
                },
                Status = x.Status,
            });

            var mappedResults = result.Select(session => _mapper.Map<GetAllSessionByAreaIdResponseModel>(session)).ToList();
            return mappedResults;
        }
    }
}
