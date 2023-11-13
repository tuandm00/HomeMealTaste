using System;
using System.Collections.Generic;

namespace HomeMealTaste.Data.Models
{
    public partial class Meal
    {
        public Meal()
        {
            MealDishes = new HashSet<MealDish>();
            MealSessions = new HashSet<MealSession>();
            Orders = new HashSet<Order>();
        }

        public int MealId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public decimal? DefaultPrice { get; set; }
        public int? KitchenId { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Kitchen? Kitchen { get; set; }
        public virtual ICollection<MealDish> MealDishes { get; set; }
        public virtual ICollection<MealSession> MealSessions { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
