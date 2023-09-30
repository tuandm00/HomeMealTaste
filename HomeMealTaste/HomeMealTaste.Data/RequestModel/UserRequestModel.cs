using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.RequestModel
{
    public class UserRequestModel
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int? Role { get; set; }
        public bool? Status { get; set; }

    }
}
