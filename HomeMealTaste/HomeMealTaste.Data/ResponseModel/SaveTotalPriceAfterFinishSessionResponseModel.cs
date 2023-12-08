using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class SaveTotalPriceAfterFinishSessionResponseModel
    {
        public int TransactionId { get; set; }
        public int? OrderId { get; set; }
        public int? WalletId { get; set; }
        public string? Date { get; set; }
        public decimal? Amount { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? TransactionType { get; set; }
        public int? UserId { get; set; }
    }
}
