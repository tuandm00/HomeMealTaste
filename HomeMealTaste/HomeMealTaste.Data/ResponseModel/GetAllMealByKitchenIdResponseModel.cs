using HomeMealTaste.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class GetAllMealByKitchenIdResponseModel
    {
        public int MealId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public double? Price { get; set; }
        public Kitchen? Kitchen { get; set; }
        public List<DishModel?> Dish { get; set; }
    }

    public class DishModel
    {
        public int DishId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public int? DishTypeId { get; set; }
        public int? KitchenId { get; set; }
    }
}
