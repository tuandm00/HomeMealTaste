using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.RequestModel
{
    public class RechargeToWalletByVNPayRequestModel
    {
        public int? UserId { get; set; }
        public double Balance { get; set; }
    }
}
