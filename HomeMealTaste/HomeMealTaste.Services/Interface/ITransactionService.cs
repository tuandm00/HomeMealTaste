﻿using HomeMealTaste.Data.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Services.Interface
{
    public interface ITransactionService
    {
        Task<List<GetAllTransactionByUserIdResponseModel>> GetAllTransactionByUserId(int userid);
        Task<List<GetAllTransactionByTransactionTypeORDERED>> GetAllTransactionByTransactionTypeORDERED();
        Task<List<GetAllTransactionByTransactionTypeRECHARGED>> GetAllTransactionByTransactionTypeRECHARGED();
        Task<List<SaveTotalPriceAfterFinishSessionResponseModel>> SaveTotalPriceAfterFinishSession(int sessionId);
    }
}
