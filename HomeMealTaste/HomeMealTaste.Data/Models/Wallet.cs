using System;
using System.Collections.Generic;

namespace HomeMealTaste.Models
{
    public partial class Wallet
    {
        public int WalletId { get; set; }
        public int? UserId { get; set; }
        public int? Balance { get; set; }
    }
}
