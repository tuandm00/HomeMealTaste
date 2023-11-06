using System;
using System.Collections.Generic;

namespace HomeMealTaste.Data.Models
{
    public partial class Feedback
    {
        public int FeedbackId { get; set; }
        public string? Description { get; set; }
        public int? CustomerId { get; set; }
        public int? KitchenId { get; set; }
    }
}
