using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class GetAllSessionResponseModel
    {
        public int SessionId { get; set; }
        public string? CreateDate { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public string? EndDate { get; set; }
        public int? UserId { get; set; }
        public string? Status { get; set; }
        public string? SessionType { get; set; }
        public string? SessionName { get; set; }
        public string? Message { get; set; }
        public List<AreaDtoGetAllSession>? AreaDtoGetAllSession { get; set; }
    }
    public class AreaDtoGetAllSession
    {
        public int AreaId { get; set; }
        public string? Address { get; set; }
        public string? AreaName { get; set; }
    }
}
