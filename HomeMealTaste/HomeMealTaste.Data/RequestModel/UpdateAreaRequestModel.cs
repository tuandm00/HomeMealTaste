using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.RequestModel
{
    public class UpdateAreaRequestModel
    {
        public int AreaId { get; set; }
        public string? Address { get; set; }
        public string? District { get; set; }
        public string? AreaName { get; set; }
    }
}
