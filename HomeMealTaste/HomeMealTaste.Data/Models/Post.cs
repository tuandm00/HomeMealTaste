using System;
using System.Collections.Generic;

namespace HomeMealTaste.Data.Models
{
    public partial class Post
    {
        public int PostId { get; set; }
        public int? OrderId { get; set; }
        public string? Status { get; set; }

        public virtual Order? Order { get; set; }
    }
}
