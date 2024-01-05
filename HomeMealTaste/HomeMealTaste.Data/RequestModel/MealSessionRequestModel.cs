using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.RequestModel
{
    public class MealSessionRequestModel
    {
        public int? MealId { get; set; }
        public List<int>? SessionIds { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public int? KitchenId { get; set; }

    }
}
