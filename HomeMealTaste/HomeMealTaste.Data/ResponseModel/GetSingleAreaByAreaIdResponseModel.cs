using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class GetSingleAreaByAreaIdResponseModel
    {
        public int AreaId { get; set; }
        public string? Address { get; set; }
        public string? AreaName { get; set; }
        public DistrictDtoGetSingleAreaByAreaId? DistrictDtoGetSingleAreaByAreaId { get; set; }
    }

    public class DistrictDtoGetSingleAreaByAreaId
    {
        public int DistrictId { get; set; }
        public string? DistrictName { get; set; }

    }
}
