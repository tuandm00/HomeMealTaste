using AutoMapper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Services.Implement
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly HomeMealTasteContext _context;
        private readonly IMapper _mapper;


        public FeedbackService(IFeedbackRepository feedbackRepository, HomeMealTasteContext context, IMapper mapper)
        {
            _feedbackRepository = feedbackRepository;
            _context = context;
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
        public async Task<FeedbackResponseModel> CreateFeedback(FeedbackRequestModel feedbackRequest)
        {
            var entity = _mapper.Map<Feedback>(feedbackRequest);

            entity.Description = feedbackRequest.Description;
            entity.CustomerId = feedbackRequest?.CustomerId;
            entity.KitchenId = feedbackRequest?.KitchenId;
            entity.CreateDate = GetDateTimeTimeZoneVietNam();

            var result = await _feedbackRepository.Create(entity, true);
            var mapped = _mapper.Map<FeedbackResponseModel>(result);

            mapped.FeedbackId = result.FeedbackId;
            mapped.CustomerId = result.CustomerId;
            mapped.KitchenId = result.KitchenId;
            mapped.Description = result.Description;
            mapped.CreateDate = GetDateTimeTimeZoneVietNam().ToString("dd-MM-yyyy");

            return mapped;
        }

        public async Task<List<FeedbackResponseModel>> GetAllFeedback()
        {
            var result = _context.Feedbacks.ToList();
            var mapped = result.Select(feedback =>
            {
                var response = _mapper.Map<FeedbackResponseModel>(feedback);
                response.CreateDate = feedback.CreateDate.ToString();
                response.Description = feedback.Description;
                response.CustomerId = feedback.CustomerId;
                response.KitchenId=feedback.KitchenId;
                return response;
            }).ToList();
            return mapped;
        }
    }
}
