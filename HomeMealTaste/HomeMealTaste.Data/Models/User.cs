using System;
using System.Collections.Generic;

namespace HomeMealTaste.Models
{
    public partial class User
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int? Role { get; set; }
    }
}
