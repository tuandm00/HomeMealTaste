using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class OrderResponseModel
    {
        public int OrderId { get; set; }
        public DateTime? Date { get; set; }
        public int? CustomerId { get; set; }
        public string? Status { get; set; }
        public int? MealId { get; set; }
        public int? SessionId { get; set; }
        public int? PromotionPrice { get; set; }
    }
}
