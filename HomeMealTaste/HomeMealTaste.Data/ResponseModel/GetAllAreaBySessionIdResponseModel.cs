using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class GetAllAreaBySessionIdResponseModel
    {
        public int AreaId { get; set; }
        public string? Address { get; set; }
        public string? AreaName { get; set; }
        public int? DistrictId { get; set; }
        public string? Status { get; set; }
        public int? SessionAreaId { get; set; }
        public int? TotalMealSessions { get; set; }
        public int? TotalOrders { get; set; }
        public int? TotalChefs { get; set; }
    }
    public class GetAllAreaResponse
    {
        public List<GetAllAreaBySessionIdResponseModel>? AreaList { get; set; }
        public int? TotalPriceOrders { get; set; }
        public int? TotalOrdersWithStatusPaid { get; set; }
        public int? TotalOrdersWithStatusAccepted { get; set; }
        public int? TotalOrdersWithStatusCompleted { get; set; }
        public int? TotalOrdersWithStatusCancelled { get; set; }
        public int? TotalOrdersWithStatusNotEat { get; set; }
        public int? TotalMealSessionWithStatusApproved { get; set; }
        public int? TotalMealSessionWithStatusCancelled { get; set; }
        public int? TotalMealSessionWithStatusMaking { get; set; }
        public int? TotalMealSessionWithStatusCompleted { get; set; }
        public int? TotalMealSessionWithStatusProcessing { get; set; }
        public int? SumTotalMealSessions { get; set; }
        public int? SumTotalOrders { get; set; }
        public int? SumTotalChefs { get; set; }
    }

}
