﻿using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Services.Interface
{
    public interface IFeedbackService
    {
         Task<FeedbackResponseModel> CreateFeedback(FeedbackRequestModel feedbackRequest);
         Task<List<FeedbackResponseModel>> GetAllFeedback();
         Task<List<FeedbackResponseModel>> GetFeedbackByKitchenId(int kitchenid);
    }
}
