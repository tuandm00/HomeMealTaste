using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class GetAllMealInMealSessionByKitchenIdResponseModel
    {
        public int MealId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public int? KitchenId { get; set; }
        public string? CreateDate { get; set; }
        public string? Description { get; set; }
    }
}
