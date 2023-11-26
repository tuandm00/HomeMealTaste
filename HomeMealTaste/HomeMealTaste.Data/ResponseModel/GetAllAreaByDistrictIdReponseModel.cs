using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class GetAllAreaByDistrictIdReponseModel
    {
        public int AreaId { get; set; }
        public string? Address { get; set; }
        public string? AreaName { get; set; }
        public DistrictDtoResponse? DistrictDtoResponse { get; set; }
    }

    public class DistrictDtoResponse
    {
        public int DistrictId { get; set; }
        public string? DistrictName { get; set; }
    }
}
