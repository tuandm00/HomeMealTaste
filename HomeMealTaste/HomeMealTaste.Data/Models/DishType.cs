using System;
using System.Collections.Generic;

namespace HomeMealTaste.Models
{
    public partial class DishType
    {
        public DishType()
        {
            Dishes = new HashSet<Dish>();
        }

        public int DishTypeId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? ChefId { get; set; }

        public virtual Chef? Chef { get; set; }
        public virtual ICollection<Dish> Dishes { get; set; }
    }
}
