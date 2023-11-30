using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.RequestModel
{
    public class RefundMoneyToWalletByOrderIdResponseModel
    {
        public int? OrderId { get; set; }
        public int? CustomerId { get; set; }
        public double? TotalPrice { get; set; }
        public int? MealSessionId { get; set; }
        public string? status { get; set; }


    }

}
