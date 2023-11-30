using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.RequestModel
{
    public class UpdateMealIdNotExistInSessionByMealIdRequestModel
    {
        public int MealId { get; set; }
        public string? Name { get; set; }
        public IFormFile? Image { get; set; }
        public int? KitchenId { get; set; }
        public string? CreateDate { get; set; }
        public string? Description { get; set; }

    }
}
