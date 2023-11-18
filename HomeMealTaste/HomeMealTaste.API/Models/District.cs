using System;
using System.Collections.Generic;

namespace HomeMealTaste.API.Models
{
    public partial class District
    {
        public District()
        {
            Areas = new HashSet<Area>();
            Customers = new HashSet<Customer>();
            Users = new HashSet<User>();
        }

        public int DistrictId { get; set; }
        public string? DistrictName { get; set; }

        public virtual ICollection<Area> Areas { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
