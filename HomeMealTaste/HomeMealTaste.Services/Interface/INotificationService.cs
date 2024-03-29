﻿using HomeMealTaste.Data.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Services.Interface
{
    public interface INotificationService
    {
       Task<ResponseModels> SendNotificationForOrderAcceptedAndMealSessionMaking(int mealSessionId);
       Task<ResponseModels> SendNotificationForOrderReadyAndMealSessionCompleted(int mealSessionId);
       Task<ResponseModels> SendNotificationForChefWhenMealSessionApproved(int mealSessionId);
       Task<ResponseModels> SendNotificationForChefWhenMealSessionRejected(int mealSessionId);
       Task<ResponseModels> SendNotificationForChefWhenMealSessionCancelled(int mealSessionId);

    }
}
