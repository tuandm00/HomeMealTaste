using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class GetAllSessionAreaByAreaIdResponseModel
    {
        public int SessionAreaId { get; set; }
        public int? SessionId { get; set; }
        public AreaDtoForSessionArea? AreaDtoForSessionArea { get; set; }
        public string? Status { get; set; }
        public List<GetAllSessionAreaByAreaIdResponseModelDto>? GetAllSessionAreaByAreaIdResponseModelDto { get; set; }
    }

    public class GetAllSessionAreaByAreaIdResponseModelDto
    {
        public KitchenDtoForSessionArea? KitchenDtoForSessionArea { get; set; }
        public int SumOfMealSession { get; set; }
        public int SumOfOrder { get; set; }
    }

    public class KitchenDtoForSessionArea
    {
        public int KitchenId { get; set; }
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public int? AreaId { get; set; }
        public int? DistrictId { get; set; }
    }

}
