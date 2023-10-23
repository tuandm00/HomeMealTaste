

using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Services.ResponseModel;

namespace HomeMealTaste.Services.Interface
{
    public interface ISessionService
    {
        Task<SessionResponseModel> CreateSession(SessionRequestModel sessionRequest);
        Task<SessionResponseModel> UpdateEndTime(int sessionId, DateTime dateTime);
    }
}
