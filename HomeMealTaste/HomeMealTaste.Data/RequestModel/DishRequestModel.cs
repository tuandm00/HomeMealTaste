using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.RequestModel
{
    public class DishRequestModel
    {
        public string? Name { get; set; }
        public string? Image { get; set; }
        public int? DishTypeId { get; set; }
        public int? KitchenId { get; set; }
    }
}
