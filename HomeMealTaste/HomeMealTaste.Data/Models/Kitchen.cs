using System;
using System.Collections.Generic;

namespace HomeMealTaste.Data.Models
{
    public partial class Kitchen
    {
        public int KitchenId { get; set; }
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? District { get; set; }
    }
}
