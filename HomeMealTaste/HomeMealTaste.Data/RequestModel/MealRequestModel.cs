using HomeMealTaste.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.RequestModel
{
    public class MealRequestModel
    {
        public int DishId { get; set; }
        public string? Name { get; set; }
        public IFormFile? Image { get; set; }
        public decimal? DefaultPrice { get; set; }
        public int? KitchenId { get; set; }


    }
}
