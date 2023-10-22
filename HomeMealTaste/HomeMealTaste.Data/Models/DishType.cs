using System;
using System.Collections.Generic;

namespace HomeMealTaste.Data.Models
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

        public virtual ICollection<Dish> Dishes { get; set; }
    }
}
