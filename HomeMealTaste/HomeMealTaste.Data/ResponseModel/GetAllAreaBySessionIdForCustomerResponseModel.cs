using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class GetAllAreaBySessionIdForCustomerResponseModel
    {
        public int AreaId { get; set; }
        public string? Address { get; set; }
        public string? AreaName { get; set; }
        public int? DistrictId { get; set; }
    }
}
