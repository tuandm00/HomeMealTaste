using System;
using System.Collections.Generic;

namespace HomeMealTaste.Data.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Feedbacks = new HashSet<Feedback>();
            Groups = new HashSet<Group>();
            Memberships = new HashSet<Membership>();
            Orders = new HashSet<Order>();
            Payments = new HashSet<Payment>();
        }

        public int CustomerId { get; set; }
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public bool? AccountStatus { get; set; }
        public string? District { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<Membership> Memberships { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
