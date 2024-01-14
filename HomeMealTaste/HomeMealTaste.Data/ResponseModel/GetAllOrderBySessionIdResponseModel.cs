using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class GetAllOrderBySessionIdResponseModel
    {
        public int OrderId { get; set; }
        public CustomerDtoForGetAllOrderBySessionId? CustomerDtoForGetAllOrderBySessionId { get; set; }
        public string? Status { get; set; }
        public MealSessionDtoGetAllOrderBySessionId? MealSessionDtoGetAllOrderBySessionId { get; set; }
        public int? TotalPrice { get; set; }
        public string? Time { get; set; }
        public int? Quantity { get; set; }
    }
    public class CustomerDtoForGetAllOrderBySessionId
    {
        public int CustomerId { get; set; }
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public int? DistrictId { get; set; }
        public AreaDtoForGetAllOrderBySessionId? AreaDtoForGetAllOrderBySessionId { get; set; }
    }
    public class MealSessionDtoGetAllOrderBySessionId
    {
        public int MealSessionId { get; set; }
        public MealDtoForGetAllOrderBySessionId? MealDtoForGetAllOrderBySessionId { get; set; }
        public SessionDtoGetAllOrderBySessionId? SessionDtoGetAllOrderBySessionId { get; set; }
        public double? Price { get; set; }
        public int? Quantity { get; set; }
        public int? RemainQuantity { get; set; }
        public string? Status { get; set; }
        public string? CreateDate { get; set; }
        public KitchenDtoForGetAllOrderBySessionId? KitchenDtoForGetAllOrderBySessionId { get; set; }
        public AreaDtoForGetAllOrderBySessionId? AreaDtoForGetAllOrderBySessionId { get; set; }
    }

    public class SessionDtoGetAllOrderBySessionId
    {
        public int SessionId { get; set; }
        public string? CreateDate { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public string? EndDate { get; set; }
        public int? UserId { get; set; }
        public string? Status { get; set; }
        public string? SessionType { get; set; }
        public string? SessionName { get; set; }
    }
    public class MealDtoForGetAllOrderBySessionId
    {
        public int MealId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public KitchenDtoForGetAllOrderBySessionId? KitchenDtoForGetAllOrderBySessionIdenId { get; set; }
        public string? CreateDate { get; set; }
        public string? Description { get; set; }
    }

    public class KitchenDtoForGetAllOrderBySessionId
    {
        public int KitchenId { get; set; }
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public int? AreaId { get; set; }
        public int? DistrictId { get; set; }
    }

    public class AreaDtoForGetAllOrderBySessionId
    {
        public int AreaId { get; set; }
        public string? Address { get; set; }
        public string? AreaName { get; set; }
        public int? DistrictId { get; set; }
    }

}
