using HomeMealTaste.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.RequestModel
{
    public class PostRequestModel
    {
        public int? OrderId { get; set; }
        public string? status { get; set; }
    }
}
