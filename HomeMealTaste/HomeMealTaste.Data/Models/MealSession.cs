using System;
using System.Collections.Generic;

namespace HomeMealTaste.Data.Models
{
    public partial class MealSession
    {
        public MealSession()
        {
            Orders = new HashSet<Order>();
        }

        public int MealSessionId { get; set; }
        public int? MealId { get; set; }
        public int? SessionId { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public int? RemainQuantity { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Meal? Meal { get; set; }
        public virtual Session? Session { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
