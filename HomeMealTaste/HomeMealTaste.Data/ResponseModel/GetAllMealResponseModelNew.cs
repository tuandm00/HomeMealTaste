using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class GetAllMealResponseModelNew
    {
        public int MealId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public KitchenDtoGetAllMealResponseModelNew? KitchenDtoGetAllMealResponseModelNew { get; set; }
        public string? CreateDate { get; set; }
        public string? Description { get; set; }
    }

    public class KitchenDtoGetAllMealResponseModelNew
    {
        public int KitchenId { get; set; }
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public int? AreaId { get; set; }
    }
}
