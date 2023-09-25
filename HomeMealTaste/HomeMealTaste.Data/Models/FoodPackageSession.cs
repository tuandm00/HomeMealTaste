using System;
using System.Collections.Generic;

namespace HomeMealTaste.Models
{
    public partial class FoodPackageSession
    {
        public FoodPackageSession()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int FoodPackageSessionId { get; set; }
        public int? FoodPackageId { get; set; }
        public int? SessionId { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public int? RemainQuantity { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual FoodPackage? FoodPackage { get; set; }
        public virtual Session? Session { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
