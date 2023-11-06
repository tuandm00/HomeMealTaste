using System;
using System.Collections.Generic;

namespace HomeMealTaste.Data.Models
{
    public partial class Membership
    {
        public int MembershipId { get; set; }
        public int? CustomerId { get; set; }
        public string? AccountRank { get; set; }
        public int? OrderId { get; set; }

        public virtual Customer? Customer { get; set; }
    }
}
