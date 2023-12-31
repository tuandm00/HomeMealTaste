﻿using System;
using System.Collections.Generic;

namespace HomeMealTaste.Data.Models
{
    public partial class Order
    {
        public Order()
        {
            Posts = new HashSet<Post>();
            Transactions = new HashSet<Transaction>();
        }

        public int OrderId { get; set; }
        public int? CustomerId { get; set; }
        public string? Status { get; set; }
        public int? MealSessionId { get; set; }
        public int? TotalPrice { get; set; }
        public DateTime? Time { get; set; }
        public int? Quantity { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual MealSession? MealSession { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
