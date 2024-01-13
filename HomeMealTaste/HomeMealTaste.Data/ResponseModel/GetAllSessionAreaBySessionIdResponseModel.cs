using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class GetAllSessionAreaBySessionIdResponseModel
    {
        public int SessionAreaId { get; set; }
        public int? SessionId { get; set; }
        public AreaDtoForSessionArea? AreaDtoForSessionArea { get; set; }
        public string? Status { get; set; }
    }

    public class AreaDtoForSessionArea
    {
        public int AreaId { get; set; }
        public string? Address { get; set; }
        public string? AreaName { get; set; }
    }
}
