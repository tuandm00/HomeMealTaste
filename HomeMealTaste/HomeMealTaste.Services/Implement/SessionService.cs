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

namespace HomeMealTaste.Services.Implement
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly HomeMealTasteContext _context;
        private readonly IMapper _mapper;

        public SessionService(HomeMealTasteContext context, ISessionRepository sessionRepository, IMapper mapper)
        {
            _context = context;
            _sessionRepository = sessionRepository;
            _mapper = mapper;
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
        public async Task<SessionResponseModel> CreateSession(SessionRequestModel sessionRequest)
        {
            var entity = _mapper.Map<Session>(sessionRequest);
            if(entity.SessionType == "Lunch")
            {
                entity.StartTime = GetDateTimeTimeZoneVietNam().Date.AddHours(10);
                entity.EndTime = entity.StartTime.Value.AddHours(2);
            }
            else if(entity.SessionType == "Evening")
            {
                entity.StartTime = GetDateTimeTimeZoneVietNam().Date.AddHours(16);
                entity.EndTime = entity.StartTime.Value.AddHours(4);
            }
            else
            {
                entity.StartTime = GetDateTimeTimeZoneVietNam().Date.AddHours(17);
                entity.EndTime = entity.StartTime.Value.AddHours(2);
            }
            
            var result = await _sessionRepository.Create(entity,true);

            var responseModel = _mapper.Map<SessionResponseModel>(result);

            responseModel.StartTime = result.StartTime?.ToString("HH:mm");
            responseModel.EndTime = result.EndTime?.ToString("HH:mm");

            return responseModel;
        }

        public async Task<SessionResponseModel> UpdateEndTime(int sessionId, DateTime endTime)
        {
            var entity = await _sessionRepository.GetFirstOrDefault(x => x.SessionId == sessionId);
            entity.EndTime = endTime;

            var response = await _sessionRepository.Update(entity);

            return _mapper.Map<SessionResponseModel>(response);

        }

        public async Task<PagedList<GetAllMealInCurrentSessionResponseModel>> GetAllMealInCurrentSession(GetAllMealRequest pagingParams)
        {
            var selectExpression = GetAllMealInCurrentSessionResponseModel.FromSession();
            var includes = new Expression<Func<Session, object>>[]
            {
                x => x.MealSessions,
                x => x.User.Kitchens
            };
            Expression<Func<Session, bool>> conditionAddition = e => e.StartTime < (pagingParams.SessionStartTime ?? DateTime.Now);
            
            var result = await _sessionRepository.GetWithPaging(pagingParams, conditionAddition, selectExpression, includes);
            
            return result;
        }

        public async Task ChangeStatusSession(int sessionid)
        {
            var result = await _context.Sessions.FindAsync(sessionid);
            if (result != null && result.Status == true)
            {
                result.Status = false;
            }
            else result.Status = true;

            await _context.SaveChangesAsync();
        }

        public async Task<List<SessionResponseModel>> GetAllSession()
        {
            var result =  _context.Sessions.ToList();
            var mapped = result.Select(session =>
            {
                var responseModel = _mapper.Map<SessionResponseModel>(session);

                responseModel.StartTime = session.StartTime?.ToString("HH:mm");
                responseModel.EndTime = session.EndTime?.ToString("HH:mm");

                return responseModel;
            }).ToList();

            return mapped;
        }
    }
}
