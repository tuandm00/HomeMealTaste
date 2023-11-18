using System;
using System.Collections.Generic;

namespace HomeMealTaste.API.Models
{
    public partial class Wallet
    {
        public Wallet()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int WalletId { get; set; }
        public int? UserId { get; set; }
        public int? Balance { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
