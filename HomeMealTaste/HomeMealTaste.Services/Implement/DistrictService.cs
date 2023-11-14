using AutoMapper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Interface;


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

        public List<DistrictResponseModel> GetAllDistrict()
        {
            var result = _districtRepository.GetAll();
            return _mapper.Map<List<DistrictResponseModel>>(result);
        }
    }
}
    