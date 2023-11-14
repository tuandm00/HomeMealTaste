using AutoMapper;
using HomeMealTaste.Data.Helper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
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

        public async Task<List<AreaResponseModel>> GetAllArea()
        {
            var result = _context.Areas.Select(x => new AreaResponseModel
            {
                AreaId = x.AreaId,
                Address = x.Address,
                District = x.District,

            }).ToList();

            var mapped = result.Select(x => _mapper.Map<AreaResponseModel>(x)).ToList();
            return mapped;
        }
    }
}
