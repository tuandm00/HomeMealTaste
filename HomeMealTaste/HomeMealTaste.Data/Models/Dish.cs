using System;
using System.Collections.Generic;

namespace HomeMealTaste.Data.Models
{
    public partial class Dish
    {
        public int DishId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public int? DishTypeId { get; set; }
        public int? KitchenId { get; set; }
    }
}
