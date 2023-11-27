using System;
using System.Collections.Generic;

namespace HomeMealTaste.Data.Models
{
    public partial class User
    {
        public User()
        {
            Customers = new HashSet<Customer>();
            Kitchens = new HashSet<Kitchen>();
            Sessions = new HashSet<Session>();
            Transactions = new HashSet<Transaction>();
            Wallets = new HashSet<Wallet>();
        }

        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int? DistrictId { get; set; }
        public int? RoleId { get; set; }
        public bool? Status { get; set; }
        public int? AreaId { get; set; }

        public virtual District? District { get; set; }
        public virtual Role? Role { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Kitchen> Kitchens { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<Wallet> Wallets { get; set; }
    }
}
