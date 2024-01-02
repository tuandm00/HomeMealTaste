using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.RequestModel
{
    public class SessionRequestModel
    {
        public string? SessionType { get; set; }
        public List<int>? AreaIds { get; set; }
        public bool? RegisterForMealStatus { get; set; }
        public bool? BookingSlotStatus { get; set; }
        public string? Date { get; set; }

    }
}
