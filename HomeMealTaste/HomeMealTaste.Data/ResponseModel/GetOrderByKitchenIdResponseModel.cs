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
        public DateTime? Date { get; set; }
        public CustomerDto? Customer { get; set; }
        public string? Status { get; set; }
        public MealSessionDto? MealSession { get; set; }
        public SessionDto? Session { get; set; }
        public int? TotalPrice { get; set; }
        public string? Time { get; set; }

    }

    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public bool? AccountStatus { get; set; }
        public int? DistrictId { get; set; }
    }

    public class MealSessionDto
    {
        public int MealSessionId { get; set; }
        public int? MealId { get; set; }
        public int? SessionId { get; set; }
        public int? Price { get; set; }
        public int? Quantity { get; set; }
        public int? RemainQuantity { get; set; }
        public string? Status { get; set; }
        public DateTime? CreateDate { get; set; }
    }

    public class SessionDto
    {
        public int SessionId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? EndDate { get; set; }
        public string? SessionName { get; set; }
        public int? UserId { get; set; }
        public bool? Status { get; set; }
        public string? SessionType { get; set; }    
    }
}
