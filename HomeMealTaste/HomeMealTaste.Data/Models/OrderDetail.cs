using System;
using System.Collections.Generic;

namespace HomeMealTaste.Data.Models
{
    public partial class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int? MealId { get; set; }
        public int? OrderId { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public int? Quantity { get; set; }
    }
}
