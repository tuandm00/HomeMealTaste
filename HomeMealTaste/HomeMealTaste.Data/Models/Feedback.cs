using System;
using System.Collections.Generic;

namespace HomeMealTaste.Models
{
    public partial class Feedback
    {
        public int FeedbackId { get; set; }
        public int? CustomerId { get; set; }
        public string? Description { get; set; }
        public int? ChefId { get; set; }

        public virtual Chef? Chef { get; set; }
        public virtual Customer? Customer { get; set; }
    }
}
