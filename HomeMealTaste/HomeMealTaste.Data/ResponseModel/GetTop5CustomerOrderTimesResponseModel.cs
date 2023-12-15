using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class GetTop5CustomerOrderTimesResponseModel
    {
        public CustomerDtoGetTop5? CustomerDtoGetTop5 { get; set;}
        public int? OrderTimes { get; set;}
    }

    public class CustomerDtoGetTop5
    {
        public int? CustomerId { get; set; }
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public int? DistrictId { get; set; }
        public int? AreaId { get; set; }
    }
}
