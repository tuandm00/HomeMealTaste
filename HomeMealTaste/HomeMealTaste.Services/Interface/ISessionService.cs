

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
        Task<SessionResponseModel> UpdateEndTime(int sessionId, DateTime dateTime);
        Task<PagedList<GetAllMealInCurrentSessionResponseModel>> GetAllMealInCurrentSession(GetAllMealRequest pagingParams);
        Task ChangeStatusSession(int sessionid);
        Task<List<SessionResponseModel>> GetAllSession();
    }
}
