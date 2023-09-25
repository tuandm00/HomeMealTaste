using System;
using System.Collections.Generic;

namespace HomeMealTaste.Models
{
    public partial class Dish
    {
        public Dish()
        {
            FoodPackageDishes = new HashSet<FoodPackageDish>();
        }

        public int DishId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public int? DishTypeId { get; set; }
        public int? ChefId { get; set; }
        public int? FoodPackageId { get; set; }

        public virtual Chef? Chef { get; set; }
        public virtual DishType? DishType { get; set; }
        public virtual FoodPackage? FoodPackage { get; set; }
        public virtual ICollection<FoodPackageDish> FoodPackageDishes { get; set; }
    }
}
