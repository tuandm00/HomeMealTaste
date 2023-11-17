using System;
using System.Collections.Generic;

namespace HomeMealTaste.API.Models
{
    public partial class Kitchen
    {
        public Kitchen()
        {
            Dishes = new HashSet<Dish>();
            Feedbacks = new HashSet<Feedback>();
            Meals = new HashSet<Meal>();
        }

        public int KitchenId { get; set; }
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Street { get; set; }
        public string? Ward { get; set; }
        public string? District { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<Dish> Dishes { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Meal> Meals { get; set; }
    }
}
