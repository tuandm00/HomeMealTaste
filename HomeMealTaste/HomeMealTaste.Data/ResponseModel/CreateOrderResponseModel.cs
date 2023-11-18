using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class CreateOrderResponseModel
    {

        public int? CustomerId { get; set; }
        public string? Status { get; set; }
        public int? MealSessionId { get; set; }
        public int? Price { get; set; }
        public int? Quantity { get; set; }
        public DateTime? Time { get; set; }
    }

    public class CustomerDtoResponse
    {
        public int CustomerId { get; set; }
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? District { get; set; }
        public int? AreaId { get; set; }
    }

    public class MealSessionDtoResponse
    {
        public int MealSessionId { get; set; }
        public MealDtoResponse? MealDtoResponse { get; set; }
        public SessionDtoResponse? SessionDtoResponse { get; set; }
        public double? Price { get; set; }
        public int? Quantity { get; set; }
        public int? RemainQuantity { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public KitchenDtoResponse? KitchenDtoResponse { get; set; }
    }

    public class MealDtoResponse
    {
        public int MealId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public KitchenDtoResponse? KitchenDtoResponse { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? Description { get; set; }
    }

    public class SessionDtoResponse
    {
        public int SessionId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? EndDate { get; set; }
        public int? UserId { get; set; }
        public bool? Status { get; set; }
        public string? SessionType { get; set; }
        public int? AreaId { get; set; }
    }

    public class KitchenDtoResponse
    {
        public int KitchenId { get; set; }
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? District { get; set; }
        public int? AreaId { get; set; }
    }
}

