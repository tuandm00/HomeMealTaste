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
using System.Text;
using System.Threading.Tasks;

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
                entity.StartTime = DateTime.Now.Date.AddHours(7);
                entity.EndTime = entity.StartTime.Value.AddHours(2);
            }
            else if(entity.SessionType == "Evening")
            {
                entity.StartTime = DateTime.Now.Date.AddHours(13);
                entity.EndTime = entity.StartTime.Value.AddHours(2);
            }
            else
            {
                entity.StartTime = DateTime.Now.Date.AddHours(17);
                entity.EndTime = entity.StartTime.Value.AddHours(2);
            }
            
            var result = await _sessionRepository.Create(entity,true);

            return _mapper.Map<SessionResponseModel>(result);

        }

        public async Task<SessionResponseModel> UpdateEndTime(int sessionId, DateTime endTime)
        {
            var entity = await _sessionRepository.GetFirstOrDefault(x => x.SessionId == sessionId);
            entity.EndTime = endTime;

            var response = await _sessionRepository.Update(entity);

            return _mapper.Map<SessionResponseModel>(response);

        }
    }
}
