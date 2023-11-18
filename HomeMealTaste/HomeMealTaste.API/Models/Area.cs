﻿using System;
using System.Collections.Generic;

namespace HomeMealTaste.API.Models
{
    public partial class Area
    {
        public Area()
        {
            Kitchens = new HashSet<Kitchen>();
            Sessions = new HashSet<Session>();
            Users = new HashSet<User>();
        }

        public int AreaId { get; set; }
        public string? Address { get; set; }
        public string? AreaName { get; set; }
        public int? DistrictId { get; set; }

        public virtual District? District { get; set; }
        public virtual ICollection<Kitchen> Kitchens { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
