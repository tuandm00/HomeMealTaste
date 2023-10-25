using AutoMapper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Services.Implement
{
    public class DistrictService : IDistrictService
    {
        private readonly IDistrictRepository _districtRepository;
        private readonly IMapper _mapper;

        public DistrictService(IDistrictRepository districtRepository, IMapper mapper)
        {
            _districtRepository = districtRepository;
            _mapper = mapper;
        }
        public async Task<DistrictResponseModel> CreateDistrict(DistrictRequestModel districtRequest)
        {
            var entity = _mapper.Map<District>(districtRequest);
            var result = await _districtRepository.Create(entity, true);

            return _mapper.Map<DistrictResponseModel>(result);
        }
    }
}
