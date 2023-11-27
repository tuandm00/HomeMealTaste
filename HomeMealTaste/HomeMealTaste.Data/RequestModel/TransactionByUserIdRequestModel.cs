using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.RequestModel
{
    public class TransactionByUserIdRequestModel
    {
        public int? OrderId { get; set; }
        public string? Date { get; set; }
        public double Amount { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? TransactionType { get; set; }
        public int? WalletId { get; set; }
        public int? UserId { get; set; }
    }
    
}
