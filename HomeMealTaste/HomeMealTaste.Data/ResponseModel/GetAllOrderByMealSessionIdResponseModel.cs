using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class GetAllOrderByMealSessionIdResponseModel
    {
        public int OrderId { get; set; }
        public CutomerDtoGetAllOrderByMealSessionId? CutomerDtoGetAllOrderByMealSessionId { get; set; }
        public string? Status { get; set; }
        public int? MealSessionId { get; set; }
        public int? TotalPrice { get; set; }
        public string? Time { get; set; }
        public int? Quantity { get; set; }
    }

    public class CutomerDtoGetAllOrderByMealSessionId
    {
        public int CustomerId { get; set; }
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public int? DistrictId { get; set; }
        public int? AreaId { get; set; }
    }
}
