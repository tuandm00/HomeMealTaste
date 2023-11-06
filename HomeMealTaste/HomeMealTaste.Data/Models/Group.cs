using System;
using System.Collections.Generic;

namespace HomeMealTaste.Data.Models
{
    public partial class Group
    {
        public int GroupId { get; set; }
        public int? KitchenId { get; set; }
        public int? SessionId { get; set; }
        public int? AreaId { get; set; }
        public int? CustomerId { get; set; }
    }
}
