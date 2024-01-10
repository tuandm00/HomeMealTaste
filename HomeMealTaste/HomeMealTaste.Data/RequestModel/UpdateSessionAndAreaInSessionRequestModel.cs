using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.RequestModel
{
    public class UpdateSessionAndAreaInSessionRequestModel
    {
        public int SessionId { get; set; }
        public string? EndDate { get; set; }
        public string? Status { get; set; }
        public string? SessionType { get; set; }
        public List<int>? AreaIds { get; set; }
    }
}
