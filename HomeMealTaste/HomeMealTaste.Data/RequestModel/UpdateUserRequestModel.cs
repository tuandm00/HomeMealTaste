using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.RequestModel
{
    public class UpdateUserRequestModel
    {
        
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public int? DistrictId { get; set; }
        public int? AreaId { get; set; }
    }
}
