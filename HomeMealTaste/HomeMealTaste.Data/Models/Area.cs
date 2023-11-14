using System;
using System.Collections.Generic;

namespace HomeMealTaste.Data.Models
{
    public partial class Area
    {
        public Area()
        {
            Customers = new HashSet<Customer>();
            Kitchens = new HashSet<Kitchen>();
            Sessions = new HashSet<Session>();
        }

        public int AreaId { get; set; }
        public string? Address { get; set; }
        public string? District { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Kitchen> Kitchens { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
    }
}
