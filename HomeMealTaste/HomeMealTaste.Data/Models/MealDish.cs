using System;
using System.Collections.Generic;

namespace HomeMealTaste.Data.Models
{
    public partial class MealDish
    {
        public int MealDishId { get; set; }
        public int? MealId { get; set; }
        public int? DishId { get; set; }
    }
}
