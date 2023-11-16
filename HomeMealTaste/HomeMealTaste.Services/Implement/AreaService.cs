using AutoMapper;
using HomeMealTaste.Data.Helper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Helper;
using HomeMealTaste.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Services.Implement
{
    public class AreaService : IAreaService
    {
        private readonly IAreaRepository _areaRepository;
        private readonly IMapper _mapper;
        private readonly HomeMealTasteContext _context;

        public AreaService(IAreaRepository areaRepository, IMapper mapper, HomeMealTasteContext context)
        {
            _areaRepository = areaRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<AreaResponseModel> CreateArea(AreaRequestModel areaRequest)
        {
            var entity = _mapper.Map<Area>(areaRequest);
            var result = await _areaRepository.Create(entity, true);
            var mapped = _mapper.Map<AreaResponseModel>(result);
            return mapped;
        }

        public Task DeleteArea(int areaid)
        {
            var result =  _areaRepository.Delete(areaid);
            return result;
        }

        public async Task<List<AreaResponseModel>> GetAllArea()
        {
            var result = _context.Areas.Select(x => new AreaResponseModel
            {
                AreaId = x.AreaId,
                Address = x.Address,
                AreaName = x.AreaName,
                DistrictDtoAreaResponseModel = new DistrictDtoAreaResponseModel
                {
                    DistrictId = x.District.DistrictId,
                    DistrictName = x.District.DistrictName,
                }
            }).ToList();

            var mapped = result.Select(x => _mapper.Map<AreaResponseModel>(x)).ToList();
            return mapped;
        }

        public Task<UpdateAreaResponseModel> UpdateArea(UpdateAreaRequestModel areaRequestModel)
        {
            var result = _context.Areas.Where(x => x.AreaId == areaRequestModel.AreaId).FirstOrDefault();
            if(result != null)
            {
                result.AreaId = areaRequestModel.AreaId;
                result.Address = areaRequestModel.Address;
                result.DistrictId = areaRequestModel.DistrictId;
                result.AreaName = areaRequestModel.AreaName;

                _context.SaveChanges();
            }

            var mapped = _mapper.Map<UpdateAreaResponseModel>(result);
            return Task.FromResult(mapped);
        }
    }
}
