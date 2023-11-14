using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Services.ResponseModel
{
    public class SessionResponseModel
    {
        public int SessionId { get; set; }
        public string? CreateDate { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public string? EndDate { get; set; }
        public int? UserId { get; set; }
        public string? SessionType { get; set; }
        public int? AreaId { get; set; }
        public bool? Status { get; set; }
    }
}
