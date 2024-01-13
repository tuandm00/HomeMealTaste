using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class GetAllSessionAreaResponseModel
    {
        public int SessionAreaId { get; set; }
        public int? SessionId { get; set; }
        public int? AreaId { get; set; }
        public string? Status { get; set; }
    }
}
