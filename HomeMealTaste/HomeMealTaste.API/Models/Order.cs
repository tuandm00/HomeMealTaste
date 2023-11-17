using System;
using System.Collections.Generic;

namespace HomeMealTaste.API.Models
{
    public partial class Order
    {
        public Order()
        {
            Posts = new HashSet<Post>();
            Transactions = new HashSet<Transaction>();
        }

        public int OrderId { get; set; }
        public DateTime? Date { get; set; }
        public int? CustomerId { get; set; }
        public string? Status { get; set; }
        public int? MealId { get; set; }
        public int? MealSessionId { get; set; }
        public int? PromotionPrice { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual MealSession? MealSession { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
