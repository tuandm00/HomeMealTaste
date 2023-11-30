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
        public string? Description { get; set; }
        public KitchenDtoReponseMeal? KitchenDtoReponseMeal { get; set; }
        public List<DishModel?> DishModel { get; set; }

    }

    public class DishModel
    {
        public int DishId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public int? DishTypeId { get; set; }
        public int? KitchenId { get; set; }
    }

    public class KitchenDtoReponseMeal
    {
        public int KitchenId { get; set; }
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public int? AreaId { get; set; }

    }
}
