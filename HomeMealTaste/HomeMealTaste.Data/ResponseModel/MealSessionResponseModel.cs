using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class MealSessionResponseModel
    {
        public int? MealSessionId { get; set; }
        public MealDtoForMealSession? MealDtoForMealSession { get; set; }
        public SessionDtoForMealSession? SessionDtoForMealSession { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public int? RemainQuantity { get; set; }
        public string? Status { get; set; }
        public string? CreateDate { get; set; }
        public KitchenDtoForMealSession? KitchenDtoForMealSession { get; set; }
    }

    public class MealDtoForMealSession
    {
        public int MealId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public int? KitchenId { get; set; }
        public string? CreateDate { get; set; }
        public string? Description { get; set; }
    }

    public class SessionDtoForMealSession
    {
        public int SessionId { get; set; }
        public string? CreateDate { get; set; } 
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public string? EndDate { get; set; }
        public int? UserId { get; set; }
        public bool? Status { get; set; }
        public string? SessionType { get; set; }
        public AreaDtoForMealSession? AreaDtoForMealSession { get; set; }
    }

    public class KitchenDtoForMealSession
    {
        public int KitchenId { get; set; }
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
    }

    public class AreaDtoForMealSession
    {
        public int AreaId { get; set; }
        public string? Address { get; set; }
        public string? AreaName { get; set; }
        public DistrictDtoForMealSession? DistrictDtoForMealSession { get; set; }
    } 

    public class DistrictDtoForMealSession
    {
        public int DistrictId { get; set; }
        public string? DistrictName { get; set; }
    }
}
