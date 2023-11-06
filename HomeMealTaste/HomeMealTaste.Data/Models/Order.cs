using System;
using System.Collections.Generic;

namespace HomeMealTaste.Data.Models
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public DateTime? Date { get; set; }
        public int? CustomerId { get; set; }
        public string? Status { get; set; }
        public int? MealId { get; set; }
        public int? SessionId { get; set; }
        public int? PromotionPrice { get; set; }
    }
}
