using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.RequestModel
{
    public class SessionForChangeStatusRequestModel
    {
        public string? SessionType { get; set; }
        public List<int>? AreaIds { get; set; }
        public string? CreateDate { get; set; }
        public string? EndDate { get; set; }
    }
}
