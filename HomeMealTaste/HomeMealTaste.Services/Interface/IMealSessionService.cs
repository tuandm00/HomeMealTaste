
using HomeMealTaste.Data.Helper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Helper;


namespace HomeMealTaste.Services.Interface
{
    public interface IMealSessionService
    {
        Task<MealSessionResponseModel> CreateMealSession(MealSessionRequestModel mealSessionRequest);
        //Task<PagedList<GetAllMealInCurrentSessionResponseModel>> GetAllMealSession(GetAllMealRequest pagingParams);
        Task<List<MealSessionResponseModel>> GetAllMealSession();
        Task<MealSessionResponseModel> GetSingleMealSessionById(int mealsessionid);

        Task<List<MealSessionResponseModel>> GetAllMealSessionByStatus(string status);

        Task UpdateStatusMeallSession (int mealsessionid, string status);

        Task<List<MealSessionResponseModel>> GetAllMeallSessionBySessionIdINDAY(int sessionid);
        Task<List<MealSessionResponseModel>> GetAllMeallSessionBySessionId(int sessionid);
        Task<List<MealSessionResponseModel>> GetAllMeallSessionByKitchenId(int kitchenid);
        Task<List<MealSessionResponseModel>> GetAllMeallSessionWithStatusAPPROVEDandREMAINQUANTITYhigherthan0InDay();
    }
}
