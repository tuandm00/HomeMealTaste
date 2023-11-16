using HomeMealTaste.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class GetMealByMealIdResponseModel
    {
        public int MealId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public KitchenDto? KitchenDto { get; set; }
        public List<DishDto?> DishDto { get; set; }
    }

    public class KitchenDto
    {
        public int KitchenId { get; set; }
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
    }

    public class DishDto
    {
        public int DishId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public DishType? DishType { get; set; }
    }
}
