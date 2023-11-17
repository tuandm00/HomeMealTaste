using System;
using System.Collections.Generic;

namespace HomeMealTaste.API.Models
{
    public partial class Post
    {
        public int PostId { get; set; }
        public string? Status { get; set; }
        public int OrderId { get; set; }

        public virtual Order Order { get; set; } = null!;
    }
}
