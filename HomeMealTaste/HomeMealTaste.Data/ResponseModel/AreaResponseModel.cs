using HomeMealTaste.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class AreaResponseModel
    {
        public int AreaId { get; set; }
        public string? Address { get; set; }
        public DistrictDtoAreaResponseModel? DistrictDtoAreaResponseModel { get; set; }
        public string? AreaName { get; set; }
        public string? Message { get; set; }


    }
    public class DistrictDtoAreaResponseModel
    {
        public int DistrictId { get; set; }
        public string? DistrictName { get; set; }
    }
}
