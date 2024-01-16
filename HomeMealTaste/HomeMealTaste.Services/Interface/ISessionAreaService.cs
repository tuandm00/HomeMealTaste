using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Services.Interface
{
    public interface ISessionAreaService
    {
        public Task<List<GetAllSessionAreaResponseModel>> GetAllSessionArea();
        public Task<List<GetAllSessionAreaBySessionIdResponseModel>> GetAllSessionAreaBySessionId(int sessionId);
        public Task<bool> ChangeStatusSessionArea(List<int> sessionArea, string status);
        public Task<bool> CheckChangeStatusSessionArea(int sessionId);
        public Task<List<GetAllSessionAreaResponseModel>> UpdateStatusSessionArea(UpdateStatusSessionAreaRequestModel request);
    }
}
