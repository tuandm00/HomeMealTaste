using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class FeedbackResponseModel
    {
        public int FeedbackId { get; set; }
        public string? Description { get; set; }
        public KitchenDtoFeedbackResponseModel KitchenDtoFeedbackResponseModel { get; set; }
        public CustomerDtoFeedbackReponseModel CustomerDtoFeedbackReponseModel { get; set; }
        public string? CreateDate { get; set; }
    }

    public class CustomerDtoFeedbackReponseModel
    {
        public int CustomerId { get; set; }
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public DistrictDtoFeedbackResponseModel DistrictDtoFeedbackResponseModel { get; set; }
    }

    public class KitchenDtoFeedbackResponseModel
    {
        public int KitchenId { get; set; }
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
    }


    public class DistrictDtoFeedbackResponseModel
    {
        public int DistrictId { get; set; }
        public string? DistrictName { get; set; }
    }
}
