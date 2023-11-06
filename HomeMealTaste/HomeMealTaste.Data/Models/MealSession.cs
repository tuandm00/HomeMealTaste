using System;
using System.Collections.Generic;

namespace HomeMealTaste.Data.Models
{
    public partial class MealSession
    {
        public int MealSessionId { get; set; }
        public int? MealId { get; set; }
        public int? SessionId { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public int? RemainQuantity { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
