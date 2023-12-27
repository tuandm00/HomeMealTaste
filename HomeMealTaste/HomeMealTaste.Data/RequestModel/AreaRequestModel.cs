using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.RequestModel
{
    public class AreaRequestModel
    {
        public string? Address { get; set; }
        public int? DistrictId { get; set; }
        public string? AreaName { get; set; }
        public List<int>? SessionIds { get; set; }

    }
}
