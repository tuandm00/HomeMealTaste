using System;
using System.Collections.Generic;

namespace HomeMealTaste.Data.Models
{
    public partial class District
    {
        public District()
        {
            Areas = new HashSet<Area>();
            Customers = new HashSet<Customer>();
            Kitchens = new HashSet<Kitchen>();
            Users = new HashSet<User>();
        }

        public int DistrictId { get; set; }
        public string? DistrictName { get; set; }

        public virtual ICollection<Area> Areas { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Kitchen> Kitchens { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
