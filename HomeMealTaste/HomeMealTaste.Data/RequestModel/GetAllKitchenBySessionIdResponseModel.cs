using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.RequestModel
{
    public  class GetAllKitchenBySessionIdResponseModel
    {
        public int? KitchenId { get; set; }
        public int? UserId { get; set; }
        public UserDtoGetAllKitchenBySessionId? UserDtoGetAllKitchenBySessionId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public AreaDtoGetAllKitchenBySessionId? AreaDtoGetAllKitchenBySessionId { get; set; }
    }

    public class UserDtoGetAllKitchenBySessionId
    {
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int? DistrictId { get; set; }
    }

    public class AreaDtoGetAllKitchenBySessionId
    {
        public int? AreaId { get; set; }
        public string? Address { get; set; }
        public string? AreaName { get; set; }
    }
}
