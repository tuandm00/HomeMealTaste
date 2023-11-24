using HomeMealTaste.Services.Library;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeMealTaste.Data.Models;
using AutoMapper;

namespace HomeMealTaste.Services.Implement
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;
        private readonly HomeMealTasteContext _context;
        private readonly IMapper _mapper;

        public VnPayService(IConfiguration configuration, HomeMealTasteContext context, IMapper mapper)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        public string CreateRechargeUrlForWallet(RechargeToWalletByVNPayRequestModel model)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var pay = new VnPayLibrary();
            var urlCallBack = _configuration["PaymentCallBack:ReturnUrl"];

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)model.Balance * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress());
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", model.UserId.ToString());
            pay.AddRequestData("vnp_OrderType", "Recharge");
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", tick);


            var paymentUrl =
                pay.CreateRequestUrl(_configuration["Vnpay:Url"], _configuration["Vnpay:HashSecret"]);

            return paymentUrl;
        }

        public async Task<PaymentResponseModel> PaymentExcute(IQueryCollection collections)
        {
            var pay = new VnPayLibrary();
            var response = pay.GetFullResponseData(collections, _configuration["VnPay:HashSecret"]);
            var userid = response.UserId;
            var balance = _context.Wallets.FirstOrDefault(x => x.UserId == userid);
            if (balance != null)
            {
                balance.Balance += (response.Balance) / 100;
                _context.Wallets.Update(balance);
                await _context.SaveChangesAsync();
            }
            return response;
        }

        
    }
}
