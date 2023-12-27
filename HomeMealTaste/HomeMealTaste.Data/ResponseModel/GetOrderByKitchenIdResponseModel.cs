using HomeMealTaste.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class GetOrderByKitchenIdResponseModel
    {
        public int OrderId { get; set; }
        public CustomerDto? Customer { get; set; }
        public string? Status { get; set; }
        public MealSessionDto? MealSession { get; set; }
        public int? TotalPrice { get; set; }
        public string? Time { get; set; }
        public int? Quantity { get; set; }

    }

    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public int? DistrictId { get; set; }
    }

    public class MealSessionDto
    {
        public int MealSessionId { get; set; }
        public MealDtoOrderResponse? MealDtoOrderResponse { get; set; }
        public SessionDto? SessionDto { get; set; }
        public int? Price { get; set; }
        public int? Quantity { get; set; }
        public int? RemainQuantity { get; set; }
        public string? Status { get; set; }
        public string? CreateDate { get; set; }
    }

    public class SessionDto
    {
        public int SessionId { get; set; }
        public string? CreateDate { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public string? EndDate { get; set; }
        public int? UserId { get; set; }
        public bool? Status { get; set; }
        public string? SessionType { get; set; }
        public AreaDtoOrderResponse? AreaDtoOrderResponse { get; set; }

    }

    public class AreaDtoOrderResponse
    {
        public int AreaId { get; set; }
        public string? Address { get; set; }
        public string? AreaName { get; set; }
        public DistrictDtoOrderResponse? DistrictDtoOrderResponse { get; set; }
    }

    public class DistrictDtoOrderResponse
    {
        public int DistrictId { get; set; }
        public string? DistrictName { get; set; }
    }

    public class MealDtoOrderResponse
    {
        public int MealId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public int? KitchenId { get; set; }
        public string? CreateDate { get; set; }
        public string? Description { get; set; }
    }
}
