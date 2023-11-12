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
        public MealDto? Meal { get; set; }
        public SessionDto? Session { get; set; }
        public int? PromotionPrice { get; set; }

    }

    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public bool? AccountStatus { get; set; }
        public string? District { get; set; }
    }

    public class MealDto
    {
        public int MealId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public decimal? DefaultPrice { get; set; }
        public int? KitchenId { get; set; }
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
