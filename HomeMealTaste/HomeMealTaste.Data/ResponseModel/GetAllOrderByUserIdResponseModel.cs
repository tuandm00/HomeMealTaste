using HomeMealTaste.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class GetAllOrderByUserIdResponseModel
    {
        public int OrderId { get; set; }
        public DateTime? Date { get; set; }
        public Customer? Customer { get; set; }
        public MealSession? MealSession { get; set; }
        public Session? Session { get; set; }
        public string? Status { get; set; }
        public int? PromotionPrice { get; set; }
    }
}
