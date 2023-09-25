using System;
using System.Collections.Generic;

namespace HomeMealTaste.Models
{
    public partial class Discount
    {
        public int DiscountId { get; set; }
        public string? DiscountCode { get; set; }
        public string? Description { get; set; }
        public int? PriceAfterDiscount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
