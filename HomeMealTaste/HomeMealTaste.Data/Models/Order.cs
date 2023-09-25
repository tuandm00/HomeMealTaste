using System;
using System.Collections.Generic;

namespace HomeMealTaste.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int OrderId { get; set; }
        public DateTime? Date { get; set; }
        public int? CustomerId { get; set; }
        public int? FoodPackageId { get; set; }
        public int? Status { get; set; }
        public int? DeliveryStatus { get; set; }
        public int? FoodPackageSessionId { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
