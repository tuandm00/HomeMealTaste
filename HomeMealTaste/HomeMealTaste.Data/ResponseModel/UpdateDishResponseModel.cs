using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class UpdateDishResponseModel
    {
        public int DishId { get; set; }
        public string? Name { get; set; }
        public IFormFile? Image { get; set; }
        public int? DishTypeId { get; set; }
        public int? KitchenId { get; set; }
    }
}
