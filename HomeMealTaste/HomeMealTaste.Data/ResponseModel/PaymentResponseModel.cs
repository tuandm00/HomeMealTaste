using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class PaymentResponseModel
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public int? UserId { get; set; }
        public int? Balance { get; set; }
        public string? VnPayResponseCode { get; set; }
        public string? urlReturnApp {  get; set; }
    }
}
