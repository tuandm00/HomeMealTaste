using System;
using System.Collections.Generic;

namespace HomeMealTaste.Data.Models
{
    public partial class Transaction
    {
        public int TransactionId { get; set; }
        public int? OrderId { get; set; }
        public int? WalletId { get; set; }
        public DateTime? Date { get; set; }
        public decimal? Amount { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? TransactionType { get; set; }

        public virtual Order? Order { get; set; }
        public virtual Wallet? Wallet { get; set; }
    }
}
