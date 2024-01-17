

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
        //Task<SessionResponseModel> CreateSession(SessionRequestModel sessionRequest);
        Task<List<SessionResponseModel>> CreateSessionWithDay(SessionRequestModel sessionRequest);
        Task<SessionResponseModel> CreateSessionForNextDay(SessionForChangeStatusRequestModel sessionRequest);
        //Task<SessionResponseModel> UpdateEndTime(int sessionId, DateTime dateTime);
        //Task<PagedList<GetAllMealInCurrentSessionResponseModel>> GetAllMealInCurrentSession(GetAllMealRequest pagingParams);
        Task ChangeStatusSession(ChangeStatusSessionRequestModel request);
        Task<List<GetAllSessionResponseModel>> GetAllSession();
        Task<List<GetAllSessionResponseModel>> GetAllSessionStatusBooking();
        Task<List<GetAllSessionByAreaIdResponseModel>> GetAllSessionByAreaId(int areaid);
        Task<List<GetAllSessionByAreaIdResponseModel>> GetAllSessionByAreaIdWithStatusOpen(int areaid);
        Task<List<SessionResponseModel>> GetAllSessionWithStatusTrueAndBookingSlotTrue();
        //Task<List<GetAllSessionByAreaIdResponseModel>> GetAllSessionByAreaIdWithStatusTrueInDay(int areaid);
        Task DeleteSession(int sessionId);
        Task<GetSingleSessionBySessionIdResponseModel> GetSingleSessionBySessionId(int sessionid);
        Task<UpdateSessionAndAreaInSessionResponseModel> UpdateSessionAndAreaInSession(UpdateSessionAndAreaInSessionRequestModel request);

    }
}
