using System;
using System.Collections.Generic;

namespace HomeMealTaste.Data.Models
{
    public partial class Kitchen
    {
        public Kitchen()
        {
            Dishes = new HashSet<Dish>();
            Feedbacks = new HashSet<Feedback>();
            MealSessions = new HashSet<MealSession>();
            Meals = new HashSet<Meal>();
        }

        public int KitchenId { get; set; }
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public int? AreaId { get; set; }

        public virtual Area? Area { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Dish> Dishes { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<MealSession> MealSessions { get; set; }
        public virtual ICollection<Meal> Meals { get; set; }
    }
}
