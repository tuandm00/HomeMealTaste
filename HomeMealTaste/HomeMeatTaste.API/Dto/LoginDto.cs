using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Services.Dto
{
    public class LoginDto
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public int? Role { get; set; }
        public string Token { get; set; }
    }
}
