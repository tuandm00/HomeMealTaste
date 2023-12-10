using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class GetTop5ChefOrderTimesResponseModel
    {
        public ChefDtoGetTop5? ChefDtoGetTop5 { get; set; }
        public int? OrderTimes { get; set; }

    }

    public class ChefDtoGetTop5
    {
        public int KitchenId { get; set; }
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public int? AreaId { get; set; }
    }
}
