﻿using System;
using System.Collections.Generic;

namespace HomeMealTaste.Data.Models
{
    public partial class Order
    {
        public Order()
        {
            Payments = new HashSet<Payment>();
            Transactions = new HashSet<Transaction>();
        }

        public int OrderId { get; set; }
        public DateTime? Date { get; set; }
        public int? CustomerId { get; set; }
        public string? Status { get; set; }
        public int? MealSessionId { get; set; }
        public int? SessionId { get; set; }
        public int? Points { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual MealSession? MealSession { get; set; }
        public virtual Session? Session { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
