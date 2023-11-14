using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class GetAllSessionByAreaIdResponseModel
    {
        public int SessionId { get; set; }
        public string? CreateDate { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public string? EndDate { get; set; }
        public int? UserId { get; set; }
        public string? SessionType { get; set; }
        public AreaDto? AreaDto { get; set; }
        public bool? Status { get; set; }
    }

    public class AreaDto
    {
        public int AreaId { get; set; }
        public string? Address { get; set; }
        public string? District { get; set; }

    }
}
