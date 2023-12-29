using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.RequestModel
{
    public class UpdateStatusMeallSessionRequestModel
    {
        public List<int>? MealSessionIds { get; set; }
        public string? status { get; set; }
    }
}
