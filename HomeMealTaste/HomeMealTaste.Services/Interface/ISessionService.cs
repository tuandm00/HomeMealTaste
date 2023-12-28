

using HomeMealTaste.Data.Helper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Helper;
using HomeMealTaste.Services.ResponseModel;

namespace HomeMealTaste.Services.Interface
{
    public interface ISessionService
    {
        Task<SessionResponseModel> CreateSession(SessionRequestModel sessionRequest);
        Task<SessionResponseModel> CreateSessionForNextDay(SessionRequestModel sessionRequest);
        //Task<SessionResponseModel> UpdateEndTime(int sessionId, DateTime dateTime);
        //Task<PagedList<GetAllMealInCurrentSessionResponseModel>> GetAllMealInCurrentSession(GetAllMealRequest pagingParams);
        Task ChangeStatusSession(int sessionid, bool status);
        Task ChangeStatusRegisterForMeal(int sessionid);
        Task ChangeStatusBookingSlot(int sessionid);
        Task<List<GetAllSessionResponseModel>> GetAllSession();
        Task<List<GetAllSessionByAreaIdResponseModel>> GetAllSessionByAreaId(int areaid);
        Task<List<GetAllSessionByAreaIdResponseModel>> GetAllSessionByAreaIdWithStatusTrue(int areaid);
        //Task<List<GetAllSessionByAreaIdResponseModel>> GetAllSessionByAreaIdWithStatusTrueInDay(int areaid);
        //Task DeleteSession(int sessionId);
        Task<GetSingleSessionBySessionIdResponseModel> GetSingleSessionBySessionId(int sessionid);

    }
}
