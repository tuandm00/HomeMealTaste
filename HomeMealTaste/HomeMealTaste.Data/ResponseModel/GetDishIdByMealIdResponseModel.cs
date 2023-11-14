using HomeMealTaste.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class GetDishIdByMealIdResponseModel
    {
        public int? MealId { get; set; }
        public Dish? Dish { get; set; }
    }
}
