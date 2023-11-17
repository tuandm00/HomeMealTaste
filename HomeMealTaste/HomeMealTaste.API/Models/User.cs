﻿using System;
using System.Collections.Generic;

namespace HomeMealTaste.API.Models
{
    public partial class User
    {
        public User()
        {
            Customers = new HashSet<Customer>();
            Kitchens = new HashSet<Kitchen>();
            Sessions = new HashSet<Session>();
        }

        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Street { get; set; }
        public string? Ward { get; set; }
        public string? District { get; set; }
        public int? RoleId { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Kitchen> Kitchens { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
    }
}
