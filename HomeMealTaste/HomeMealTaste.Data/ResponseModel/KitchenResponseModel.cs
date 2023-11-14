using HomeMealTaste.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class KitchenResponseModel
    {
        public int KitchenId { get; set; }
        public User? User { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? District { get; set; }
    }
}
