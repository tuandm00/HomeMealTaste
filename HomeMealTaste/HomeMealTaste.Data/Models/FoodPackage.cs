using System;
using System.Collections.Generic;

namespace HomeMealTaste.Models
{
    public partial class FoodPackage
    {
        public FoodPackage()
        {
            Dishes = new HashSet<Dish>();
            FoodPackageDishes = new HashSet<FoodPackageDish>();
            FoodPackageSessions = new HashSet<FoodPackageSession>();
            OrderItems = new HashSet<OrderItem>();
        }

        public int FoodPackageId { get; set; }
        public string? Name { get; set; }
        public byte[]? Image { get; set; }
        public decimal? DefaultPrice { get; set; }

        public virtual ICollection<Dish> Dishes { get; set; }
        public virtual ICollection<FoodPackageDish> FoodPackageDishes { get; set; }
        public virtual ICollection<FoodPackageSession> FoodPackageSessions { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
