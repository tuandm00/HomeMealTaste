using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class GetAllKitchenByAreaIdResponseModel
    {
        public int KitchenId { get; set; }
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public AreaDtoGetAllKitchenByAreaId? AreaDtoGetAllKitchenByAreaId { get; set; }
        public List<MealSessionDtoGetAllKitchenByAreaId>? MealSessionDtoGetAllKitchenByAreaId { get; set; }
        public int? DistrictId { get; set; }
    }

    public class AreaDtoGetAllKitchenByAreaId
    {
        public int? AreaId { get; set; }
        public string? Address { get; set; }
        public string? AreaName { get; set; }
    }

    public class MealSessionDtoGetAllKitchenByAreaId
    {
        public int MealSessionId { get; set; }
        public int? MealId { get; set; }
        public int? SessionId { get; set; }
        public double? Price { get; set; }
        public int? Quantity { get; set; }
        public int? RemainQuantity { get; set; }
        public string? Status { get; set; }
        public string? CreateDate { get; set; }
        public int? KitchenId { get; set; }
    }
}
