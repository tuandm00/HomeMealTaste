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
        }

        public int MealId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public decimal? DefaultPrice { get; set; }

        public virtual ICollection<MealDish> MealDishes { get; set; }
        public virtual ICollection<MealSession> MealSessions { get; set; }
    }
}
