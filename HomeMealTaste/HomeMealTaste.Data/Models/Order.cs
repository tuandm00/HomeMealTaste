using System;
using System.Collections.Generic;

namespace HomeMealTaste.Data.Models
{
    public partial class Order
    {
        public Order()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int OrderId { get; set; }
        public DateTime? Date { get; set; }
        public int? CustomerId { get; set; }
        public string? Status { get; set; }
        public int? MealSessionId { get; set; }
        public int? Price { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual MealSession? MealSession { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
