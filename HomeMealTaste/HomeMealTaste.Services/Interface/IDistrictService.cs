using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;

namespace HomeMealTaste.Services.Interface
{
    public interface IDistrictService
    {
        Task<DistrictResponseModel> CreateDistrict(DistrictRequestModel districtRequest);
        Task<DistrictResponseModel> UpdateDistrict(UpdateDistrictRequestModel districtRequest);
        Task<DistrictResponseModel> DeleteDistrict(int districtId);
        List<DistrictResponseModel> GetAllDistrict();
    }
}
