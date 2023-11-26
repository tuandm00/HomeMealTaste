using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.RequestModel
{
    public class PaymentInformationResquestModel
    {
        public string? OrderId { get; set; }
        public string? CustomerId { get; set; }
        public double Amount { get; set; }
    }
}
