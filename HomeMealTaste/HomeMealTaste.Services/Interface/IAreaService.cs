using HomeMealTaste.Data.Helper;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Services.Interface
{
    public interface IAreaService
    {
        Task<List<AreaResponseModel>> GetAllArea();
        Task<AreaResponseModel> CreateArea(AreaRequestModel areaRequest);
        Task DeleteArea(int areaid);
        Task<UpdateAreaResponseModel> UpdateArea(UpdateAreaRequestModel updateAreaRequest);
    }
}
