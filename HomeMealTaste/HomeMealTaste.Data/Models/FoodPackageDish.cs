using System;
using System.Collections.Generic;

namespace HomeMealTaste.Models
{
    public partial class FoodPackageDish
    {
        public int FoodPackageDishId { get; set; }
        public int? FoodPackageId { get; set; }
        public int? DishId { get; set; }

        public virtual Dish? Dish { get; set; }
        public virtual FoodPackage? FoodPackage { get; set; }
    }
}
