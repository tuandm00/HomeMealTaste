using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.RequestModel
{
    public class UpdateStatusSessionAreaRequestModel
    {
        public List<int>? SessionAreaIds { get; set; }
        public string? Status { get; set; }
    }
}
