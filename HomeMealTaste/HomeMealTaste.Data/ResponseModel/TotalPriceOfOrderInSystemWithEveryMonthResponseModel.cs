using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class TotalPriceOfOrderInSystemWithEveryMonthResponseModel
    {
        public int? Month { get; set; }
        public int? TotalPrice { get; set; }
    }
}
