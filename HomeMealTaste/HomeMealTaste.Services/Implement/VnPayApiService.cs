using HomeMealTaste.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static HomeMealTaste.Services.Implement.VnPayApiService;

namespace HomeMealTaste.Services.Implement
{
    public class VnPayApiService
    {
        private readonly IConfiguration _configuration;

        public VnPayApiService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public class VnPayLibrary
        {
            public const string VERSION = "2.1.0";
            private SortedList<String, String> _requestData = new SortedList<String, String>(new VnPayCompare());
            private SortedList<String, String> _responseData = new SortedList<String, String>(new VnPayCompare());

            public void AddRequestData(string key, string value)
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _requestData.Add(key, value);
                }
            }

            public void AddResponseData(string key, string value)
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _responseData.Add(key, value);
                }
            }

            public string GetResponseData(string key)
            {
                string retValue;
                if (_responseData.TryGetValue(key, out retValue))
                {
                    return retValue;
                }
                else
                {
                    return string.Empty;
                }
            }

            #region Request

            public string CreateRequestUrl(string baseUrl, string vnp_HashSecret)
            {
                StringBuilder data = new StringBuilder();
                foreach (KeyValuePair<string, string> kv in _requestData)
                {
                    if (!String.IsNullOrEmpty(kv.Value))
                    {
                        data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                    }
                }
                string queryString = data.ToString();

                baseUrl += "?" + queryString;
                String signData = queryString;
                if (signData.Length > 0)
                {

                    signData = signData.Remove(data.Length - 1, 1);
                }
                string vnp_SecureHash = Utils.HmacSHA512(vnp_HashSecret, signData);
                baseUrl += "vnp_SecureHash=" + vnp_SecureHash;

                return baseUrl;
            }



            #endregion

            #region Response process

            public bool ValidateSignature(string inputHash, string secretKey)
            {
                string rspRaw = GetResponseData();
                string myChecksum = Utils.HmacSHA512(secretKey, rspRaw);
                return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
            }
            private string GetResponseData()
            {

                StringBuilder data = new StringBuilder();
                if (_responseData.ContainsKey("vnp_SecureHashType"))
                {
                    _responseData.Remove("vnp_SecureHashType");
                }
                if (_responseData.ContainsKey("vnp_SecureHash"))
                {
                    _responseData.Remove("vnp_SecureHash");
                }
                foreach (KeyValuePair<string, string> kv in _responseData)
                {
                    if (!String.IsNullOrEmpty(kv.Value))
                    {
                        data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                    }
                }
                //remove last '&'
                if (data.Length > 0)
                {
                    data.Remove(data.Length - 1, 1);
                }
                return data.ToString();
            }
            #endregion
        }

        public class Utils
        {


            public static String HmacSHA512(string key, String inputData)
            {
                var hash = new StringBuilder();
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
                using (var hmac = new HMACSHA512(keyBytes))
                {
                    byte[] hashValue = hmac.ComputeHash(inputBytes);
                    foreach (var theByte in hashValue)
                    {
                        hash.Append(theByte.ToString("x2"));
                    }
                }

                return hash.ToString();
            }
            public static string GetIpAddress()
            {

                return "123.0.0.1";
            }
        }

        public class VnPayCompare : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                if (x == y) return 0;
                if (x == null) return -1;
                if (y == null) return 1;
                var vnpCompare = CompareInfo.GetCompareInfo("en-US");
                return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
            }
        }

        public string GeneratePaymentLink()
        {
            //Build URL for VNPAY

            OrderInfo order = new OrderInfo();
            order.OrderId = DateTime.Now.Ticks; // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
            order.Amount = 100000; // Giả lập số tiền thanh toán hệ thống merchant gửi sang VNPAY 100,000 VND
            order.Status = "0"; //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending" khởi tạo giao dịch chưa có IPN
            order.CreatedDate = DateTime.Now;
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", "V25Y8STO");
            vnpay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000

            vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_BankCode", "NCB");
            vnpay.AddRequestData("vnp_Locale", "vn");

            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + order.OrderId);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", "https://localhost:7294/api/Payment/createpaymentlink");
            vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày


            //Billing

            string paymentUrl = vnpay.CreateRequestUrl("https://sandbox.vnpayment.vn/paymentv2/vpcpay.html", "XCVUBTSMXMAJUGQEFKBZQYXGJPPZBNGX");
            return paymentUrl;
        }

        public class OrderInfo
        {
            public long OrderId { get; set; }
            public long Amount { get; set; }
            public string OrderDesc { get; set; }

            public DateTime CreatedDate { get; set; }
            public string Status { get; set; }

            public long PaymentTranId { get; set; }
            public string BankCode { get; set; }
            public string PayStatus { get; set; }


        }

        //private string GetIpAddress()
        //{
        //    // Implement a method to get the user's IP address
        //    // You may use HttpContextAccessor or any other method based on your application
        //    // For simplicity, I'm returning a dummy IP here
        //    return "192.168.1.30";
        //}

        //private string ToQueryString(Dictionary<string, string> parameters)
        //{
        //    return string.Join("&", parameters.Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}"));
        //}


        //public static class EncryptHelper
        //{
        //    public static string MD5Hash(string input)
        //    {
        //        using (var md5 = System.Security.Cryptography.MD5.Create())
        //        {
        //            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
        //            byte[] hashBytes = md5.ComputeHash(inputBytes);

        //            StringBuilder sb = new StringBuilder();
        //            for (int i = 0; i < hashBytes.Length; i++)
        //            {
        //                sb.Append(hashBytes[i].ToString("X2"));
        //            }

        //            return sb.ToString();
        //        }
        //    }

        //public static string HmacSha512Hash(string data, string key)
        //{
        //    using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key)))
        //    {
        //        byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        //        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        //    }
        //}
        //public static string Sha256Hash(string data)
        //{
        //    using (var sha256 = SHA256.Create())
        //    {
        //        byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
        //        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        //    }
        //}
    }
}


