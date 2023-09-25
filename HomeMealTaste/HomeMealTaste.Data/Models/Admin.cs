using System;
using System.Collections.Generic;

namespace HomeMealTaste.Models
{
    public partial class Admin
    {
        public int AdminId { get; set; }
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
