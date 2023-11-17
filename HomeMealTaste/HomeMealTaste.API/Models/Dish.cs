using System;
using System.Collections.Generic;

namespace HomeMealTaste.API.Models
{
    public partial class Dish
    {
        public Dish()
        {
            MealDishes = new HashSet<MealDish>();
        }

        public int DishId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public int? DishTypeId { get; set; }
        public int? KitchenId { get; set; }

        public virtual DishType? DishType { get; set; }
        public virtual Kitchen? Kitchen { get; set; }
        public virtual ICollection<MealDish> MealDishes { get; set; }
    }
}
