using System;
using System.Collections.Generic;

namespace HomeMealTaste.Data.Models
{
    public partial class Kitchen
    {
        public Kitchen()
        {
            Dishes = new HashSet<Dish>();
            Groups = new HashSet<Group>();
        }

        public int KitchenId { get; set; }
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public bool? AccountStatus { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<Dish> Dishes { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
    }
}
