using System;
using System.Collections.Generic;

namespace HomeMealTaste.Data.Models
{
    public partial class Membership
    {
        public Membership()
        {
            Customers = new HashSet<Customer>();
        }

        public int MembershipId { get; set; }
        public string? AccountRank { get; set; }
        public int? OrderId { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
