using AutoMapper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Interface;
using Microsoft.EntityFrameworkCore;
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
            _context.Entry(result).Reference(f => f.Customer).Load();
            _context.Entry(result).Reference(f => f.Kitchen).Load();

            var mapped = _mapper.Map<FeedbackResponseModel>(result);

            //mapped.FeedbackId = result.FeedbackId;
            //mapped.CustomerDtoFeedbackReponseModel = new CustomerDtoFeedbackReponseModel
            //{
            //    CustomerId = result.Customer.CustomerId,
            //    UserId = result.Customer.UserId,
            //    Name = result.Customer.Name,
            //    Phone = result.Customer.Phone,
            //    DistrictDtoFeedbackResponseModel = new DistrictDtoFeedbackResponseModel
            //    {
            //        DistrictId = result.Customer.District.DistrictId,
            //        DistrictName = result.Customer.District.DistrictName,
            //    },
            //};
            //mapped.KitchenDtoFeedbackResponseModel = new KitchenDtoFeedbackResponseModel
            //{
            //    KitchenId = result.Kitchen.KitchenId,
            //    UserId = result.Kitchen.UserId,
            //    Name = result.Kitchen.Name,
            //    Address = result.Kitchen.Address,
            //};
            //mapped.Description = result.Description;
            //mapped.CreateDate = GetDateTimeTimeZoneVietNam().ToString("dd-MM-yyyy");

            return mapped;
        }

        public async Task<List<FeedbackResponseModel>> GetAllFeedback()
        {
            var result = _context.Feedbacks.Include(x => x.Customer).ThenInclude(x => x.User).ThenInclude(x => x.District)
                .Include(x => x.Kitchen).ToList();
            var mapped = result.Select(feedback =>
            {
                var response = _mapper.Map<FeedbackResponseModel>(feedback);
                response.CreateDate = feedback.CreateDate.ToString();
                response.Description = feedback.Description;
                response.CustomerDtoFeedbackReponseModel = new CustomerDtoFeedbackReponseModel
                {
                    CustomerId = feedback.Customer.CustomerId,
                    UserId = feedback.Customer.UserId,
                    Name = feedback.Customer.Name,
                    Phone = feedback.Customer.Phone,
                    DistrictDtoFeedbackResponseModel = new DistrictDtoFeedbackResponseModel
                    {
                        DistrictId = feedback.Customer.District.DistrictId,
                        DistrictName = feedback.Customer.District.DistrictName,
                    },
                };
                response.KitchenDtoFeedbackResponseModel = new KitchenDtoFeedbackResponseModel
                {
                    KitchenId = feedback.Kitchen.KitchenId,
                    UserId = feedback.Kitchen.UserId,
                    Name = feedback.Kitchen.Name,
                    Address = feedback.Kitchen.Address,
                };
                return response;
            }).ToList();
            return mapped;
        }

        public async Task<List<FeedbackResponseModel>> GetFeedbackByKitchenId(int kitchenid)
        {
            var result = _context.Feedbacks
                .Include(x => x.Customer).ThenInclude(x => x.User).ThenInclude(x => x.District)
                .Include(x => x.Kitchen)
                .Where(x => x.KitchenId == kitchenid).Select(x => new FeedbackResponseModel
                {
                    FeedbackId = x.FeedbackId,
                    Description = x.Description,
                    CreateDate = ((DateTime)x.CreateDate).ToString("dd-MM-yyyy"),
                    CustomerDtoFeedbackReponseModel = new CustomerDtoFeedbackReponseModel
                    {
                        CustomerId = x.Customer.CustomerId,
                        Name = x.Customer.Name,
                        Phone = x.Customer.Phone,
                        UserId = x.Customer.UserId,
                        DistrictDtoFeedbackResponseModel = new DistrictDtoFeedbackResponseModel
                        {
                            DistrictId = x.Customer.District.DistrictId,
                            DistrictName = x.Customer.District.DistrictName,
                        }
                    },
                    KitchenDtoFeedbackResponseModel = new KitchenDtoFeedbackResponseModel
                    {
                        KitchenId = x.Kitchen.KitchenId,
                        Name = x.Kitchen.Name,
                        Address = x.Kitchen.Address,
                        UserId = x.Kitchen.UserId,
                    }
                }).ToList();
            var mapped = result.Select(result => _mapper.Map<FeedbackResponseModel>(result)).ToList();
            return mapped;
        }
    }
}
