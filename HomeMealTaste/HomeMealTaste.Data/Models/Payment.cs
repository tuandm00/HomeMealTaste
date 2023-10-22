using System;
using System.Collections.Generic;

namespace HomeMealTaste.Data.Models
{
    public partial class Payment
    {
        public int PaymentId { get; set; }
        public string? Method { get; set; }
        public DateTime? Time { get; set; }
        public string? Status { get; set; }
        public int? CustomerId { get; set; }
        public int? WalletId { get; set; }
        public int? TransactionId { get; set; }
        public int? OrderId { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Order? Order { get; set; }
        public virtual Transaction? Transaction { get; set; }
        public virtual Wallet? Wallet { get; set; }
    }
}
