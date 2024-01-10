using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.RequestModel
{
    public class GetSingleMealSessionByIdResponseModel
    {
        public int? MealSessionId { get; set; }
        public MealDtoForMealSessions? MealDtoForMealSessions { get; set; }
        public SessionDtoForMealSessions? SessionDtoForMealSessions { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public int? RemainQuantity { get; set; }
        public string? Status { get; set; }
        public string? CreateDate { get; set; }
        public KitchenDtoForMealSessions? KitchenDtoForMealSessions { get; set; }
    }

    public class MealDtoForMealSessions
    {
        public int MealId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public int? KitchenId { get; set; }
        public string? CreateDate { get; set; }
        public string? Description { get; set; }
        public List<DishesDtoMealSession>? DishesDtoMealSession { get; set; }
    }

    public class SessionDtoForMealSessions
    {
        public int SessionId { get; set; }
        public string? CreateDate { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public string? EndDate { get; set; }
        public int? UserId { get; set; }
        public string? Status { get; set; }
        public string? SessionType { get; set; }
        public AreaDtoForMealSessions? AreaDtoForMealSessions { get; set; }
    }

    public class KitchenDtoForMealSessions
    {
        public int KitchenId { get; set; }
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
    }

    public class AreaDtoForMealSessions
    {
        public int AreaId { get; set; }
        public string? Address { get; set; }
        public string? AreaName { get; set; }
        public DistrictDtoForMealSessions? DistrictDtoForMealSessions { get; set; }
    }

    public class DistrictDtoForMealSessions
    {
        public int DistrictId { get; set; }
        public string? DistrictName { get; set; }
    }


    public class DishesDtoMealSession
    {
        public int DishId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public DishTypeDtoMealSession? DishTypeDtoMealSession { get; set; }
        public int? KitchenId { get; set; }
    }

    public class DishTypeDtoMealSession
    {
        public int DishTypeId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
