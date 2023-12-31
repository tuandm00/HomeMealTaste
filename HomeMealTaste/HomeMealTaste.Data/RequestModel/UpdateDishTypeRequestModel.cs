using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.RequestModel
{
    public class UpdateDishTypeRequestModel
    {
        public int DishTypeId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
