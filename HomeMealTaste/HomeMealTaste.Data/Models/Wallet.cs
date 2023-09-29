using System;
using System.Collections.Generic;

namespace HomeMealTaste.Data.Models
{
    public partial class Wallet
    {
        public Wallet()
        {
            Payments = new HashSet<Payment>();
            Transactions = new HashSet<Transaction>();
        }

        public int WalletId { get; set; }
        public int? UserId { get; set; }
        public int? Balance { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
