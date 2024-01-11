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
        public Task<List<GetAllSessionAreaResponseModel>> UpdateStatusSessionArea(UpdateStatusSessionAreaRequestModel request);
    }
}
