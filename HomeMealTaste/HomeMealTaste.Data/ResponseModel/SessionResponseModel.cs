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
        public String? CreateDate { get; set; }
        public String? StartTime { get; set; }
        public String? EndTime { get; set; }
        public String? EndDate { get; set; }
        public int? UserId { get; set; }
        public bool? Status { get; set; }
    }
}
