using System;
using System.Collections.Generic;

namespace HomeMealTaste.Data.Models
{
    public partial class Post
    {
        public int PostId { get; set; }
        public int? MealSessionId { get; set; }
        public string? Status { get; set; }

        public virtual MealSession? MealSession { get; set; }
    }
}
