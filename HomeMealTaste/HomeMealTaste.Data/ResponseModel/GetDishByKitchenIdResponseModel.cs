using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class GetDishByKitchenIdResponseModel
    {
        public int DishId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public DishTypeResponse? DishTypeResponse { get; set; }
        public int? KitchenId { get; set; }
    }

    public class DishTypeResponse
    {
        public int DishTypeId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
