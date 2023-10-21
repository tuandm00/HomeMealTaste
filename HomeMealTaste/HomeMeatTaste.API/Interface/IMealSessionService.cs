
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;


namespace HomeMealTaste.Services.Interface
{
    public interface IMealSessionService
    {
        Task<MealSessionResponseModel> CreateMealSession(MealSessionRequestModel mealSessionRequest);
    }
}
