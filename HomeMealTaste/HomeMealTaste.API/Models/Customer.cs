using System;
using System.Collections.Generic;

namespace HomeMealTaste.API.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Feedbacks = new HashSet<Feedback>();
            Orders = new HashSet<Order>();
        }

        public int CustomerId { get; set; }
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public bool? AccountStatus { get; set; }
        public string? Street { get; set; }
        public string? Ward { get; set; }
        public string? District { get; set; }
        public int? OrderDetailId { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
