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
        private readonly HomeMealTasteContext _context;
        private readonly IMapper _mapper;

        public DistrictService(IDistrictRepository districtRepository, IMapper mapper, HomeMealTasteContext context)
        {
            _districtRepository = districtRepository;
            _mapper = mapper;
            _context = context;
        }
        public async Task<DistrictResponseModel> CreateDistrict(DistrictRequestModel districtRequest)
        {
            var entity = _mapper.Map<District>(districtRequest);
            var result = await _districtRepository.Create(entity, true);

            return _mapper.Map<DistrictResponseModel>(result);
        }

        public async Task<DistrictResponseModel> DeleteDistrict(int districtId)
        {
            var result = await _context.Districts.FindAsync(districtId);
            if(result != null)
            {
                _context.Districts.Remove(result);
            }
            await _context.SaveChangesAsync();
            var mapped = _mapper.Map<DistrictResponseModel>(result);
            return mapped;
        }

        public List<DistrictResponseModel> GetAllDistrict()
        {
            var result = _districtRepository.GetAll();
            return _mapper.Map<List<DistrictResponseModel>>(result);
        }

        public async Task<DistrictResponseModel> UpdateDistrict(UpdateDistrictRequestModel districtRequest)
        {
            var existingEntity = await _context.Districts.FindAsync(districtRequest.DistrictId);

            if (existingEntity != null)
            {
                _mapper.Map(districtRequest, existingEntity);

                // Save the changes to the database
                await _context.SaveChangesAsync();
            }
            else
            {
                return null;
            }

            var mapped = _mapper.Map<DistrictResponseModel>(existingEntity);
            return mapped;
        }
    }
}
    