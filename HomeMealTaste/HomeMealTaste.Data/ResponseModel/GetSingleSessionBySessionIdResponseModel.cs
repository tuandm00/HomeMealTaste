using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class GetSingleSessionBySessionIdResponseModel
    {
        public int SessionId { get; set; }
        public string? CreateDate { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public string? EndDate { get; set; }
        public UserDtoGetSingleSessionBySessionId? UserDtoGetSingleSessionBySessionId { get; set; }
        public bool? Status { get; set; }
        public string? SessionType { get; set; }
        public string? SessionName { get; set; }
        public List<AreaDtoGetSingleSessionBySessionId>? AreaDtoGetSingleSessionBySessionId { get; set; }
    }

    public class UserDtoGetSingleSessionBySessionId
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int? DistrictId { get; set; }

    }
    public class AreaDtoGetSingleSessionBySessionId
    {
        public int AreaId { get; set; }
        public string? Address { get; set; }
        public string? AreaName { get; set; }
    }
}
