using System;
using System.Collections.Generic;

namespace HomeMealTaste.Models
{
    public partial class OrderItem
    {
        public int OrderItemId { get; set; }
        public int? FoodPackageId { get; set; }
        public int? OrderId { get; set; }
        public int? Quantity { get; set; }
        public int? FoodPackageSessionId { get; set; }

        public virtual FoodPackage? FoodPackage { get; set; }
        public virtual FoodPackageSession? FoodPackageSession { get; set; }
        public virtual Order? Order { get; set; }
    }
}
