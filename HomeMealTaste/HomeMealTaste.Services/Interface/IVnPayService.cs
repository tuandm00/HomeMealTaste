using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Services.Interface
{
    public interface IVnPayService
    {
        string CreateRechargeUrlForWallet(RechargeToWalletByVNPayRequestModel model);
        Task<PaymentResponseModel> PaymentExcute(IQueryCollection collections);

    }
}
