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
        public string? Date { get; set; }
        public bool? Status { get; set; }
        public string? SessionType { get; set; }
        public bool? RegisterForMealStatus { get; set; }
        public bool? BookingSlotStatus { get; set; }
        public List<int>? AreaIds { get; set; }
    }
}
