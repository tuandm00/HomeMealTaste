using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.RequestModel
{
    public class FeedbackRequestModel
    {
        public string? Description { get; set; }
        public int? CustomerId { get; set; }
        public int? KitchenId { get; set; }
        public string? CreateDate { get; set; }

    }
}
