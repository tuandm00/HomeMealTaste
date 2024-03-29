﻿
using HomeMealTaste.Data.Helper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Helper;


namespace HomeMealTaste.Services.Interface
{
    public interface IMealSessionService
    {
        Task<List<MealSessionResponseModel>> CreateMealSession(MealSessionRequestModel mealSessionRequest);
        ////Task<PagedList<GetAllMealInCurrentSessionResponseModel>> GetAllMealSession(GetAllMealRequest pagingParams);
        Task<List<MealSessionResponseModel>> GetAllMealSession();
        Task<GetSingleMealSessionByIdResponseModel> GetSingleMealSessionById(int mealsessionid);

        Task<List<MealSessionResponseModel>> GetAllMealSessionByStatus(string status);
        Task<List<MealSessionResponseModel>> GetAllMealSessionWithStatusProcessingByKitchenId(int kitchenId);
        Task<List<MealSessionResponseModel>> GetAllMealSessionByMealId(int mealId);
        Task UpdateStatusMeallSession(UpdateStatusMeallSessionRequestModel request);
        Task UpdateAreaAndAllMealSessionWithStatusProcessing(UpdateAreaAndAllMealSessionWithStatusProcessingRequestModel request);

        Task<List<MealSessionResponseModel>> GetAllMeallSessionBySessionIdINDAY(int sessionid);
        Task<List<MealSessionResponseModel>> GetAllMeallSessionBySessionId(int sessionid);
        Task<List<MealSessionResponseModel>> GetAllMeallSessionWithNotStatusCompletedAndCancelledByKitchenId(int kitchenId);
        Task<List<MealSessionResponseModel>> GetAllMeallSessionBySessionIdWithStatusApprovedandREMAINQUANTITYhigherthan0InDay(int sessionid);
        Task<List<MealSessionResponseModel>> GetAllMeallSessionByKitchenIdWithStatusApprovedandREMAINQUANTITYhigherthan0InDay(int kitchenid);
        Task<List<MealSessionResponseModel>> GetAllMeallSessionByKitchenId(int kitchenid);
        Task<List<MealSessionResponseModel>> GetAllMeallSessionByKitchenIdInSession(int kitchenid, int sessionid);
        Task<List<MealSessionResponseModel>> GetAllMeallSessionWithStatusAPPROVEDandREMAINQUANTITYhigherthan0InDay();
        Task<List<MealSessionResponseModel>> GetAllMeallSessionBySessionIdINDAYAndByKitchenId(int sessionid, int kitchenid);
        Task<List<MealSessionResponseModel>> GetAllMealSessionByAreaIdAndSessionIdAndKitchenId(int areaId,int sessionId, int kitchenId);
        Task<List<MealSessionResponseModel>> GetAllMeallSessionWithStatusCompletedByKitchenId(int kitchenId);


    }
}
