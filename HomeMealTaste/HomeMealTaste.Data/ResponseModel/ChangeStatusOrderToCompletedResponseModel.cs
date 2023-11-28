using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class ChangeStatusOrderToCompletedResponseModel
    {
        public int OrderId { get; set; }
        public int? CustomerId { get; set; }
        public string? Status { get; set; }
        public int? MealSessionId { get; set; }
        public int? TotalPrice { get; set; }
        public string? Time { get; set; }
        public int? Quantity { get; set; }
    }
}
