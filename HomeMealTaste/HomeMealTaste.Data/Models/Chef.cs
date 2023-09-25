using System;
using System.Collections.Generic;

namespace HomeMealTaste.Models
{
    public partial class Chef
    {
        public Chef()
        {
            DishTypes = new HashSet<DishType>();
            Dishes = new HashSet<Dish>();
            Feedbacks = new HashSet<Feedback>();
        }

        public int ChefId { get; set; }
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public bool? AccountStatus { get; set; }

        public virtual ICollection<DishType> DishTypes { get; set; }
        public virtual ICollection<Dish> Dishes { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
    }
}
