using AutoMapper;
using HomeMealTaste.Data.Helper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Interface;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.EntityFrameworkCore;


namespace HomeMealTaste.Services.Implement
{
    public class KitchenService : IKitchenService
    {
        private readonly IKitchenRepository _kitchenRepository;
        private readonly IMapper _mapper;
        private readonly HomeMealTasteContext _context;


        public KitchenService(IKitchenRepository kitchenRepository, IMapper mapper, HomeMealTasteContext context)
        {
            _kitchenRepository = kitchenRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<KitchenResponseModel>> GetAllKitchen()
        {
            var result = await _context.Kitchens
        .Include(x => x.User.Wallets)
        .Include(x => x.District)
        .Include(x => x.User.Area)
        .Select(x => new KitchenResponseModel
        {
            KitchenId = x.KitchenId,
            Name = x.Name,
            Address = x.Address,
            UserDtoKitchenResponseModel = new UserDtoKitchenResponseModel
            {
                UserId = x.User.UserId,
                Username = x.User.Username,
                Email = x.User.Email,
                Phone = x.User.Phone,
                WalletDtoKitchenResponseModel = new WalletDtoKitchenResponseModel
                {
                    WalletId = x.User.Wallets.FirstOrDefault().WalletId,
                    Balance = x.User.Wallets.FirstOrDefault().Balance,
                    UserId = x.User.Wallets.FirstOrDefault().UserId,
                },
            },
            DistrictDtoGetKitchen = new DistrictDtoGetKitchen
            {
                DistrictId = x.District.DistrictId,
                DistrictName = x.District.DistrictName,
            },
            AreaDtoGetKitchen = new AreaDtoGetKitchen
            {
                AreaId = x.User.Area.AreaId,
                Address = x.User.Area.Address,
                AreaName = x.User.Area.AreaName,
            },
        }).ToListAsync();

            var mapped = result.Select(results => _mapper.Map<KitchenResponseModel>(results)).ToList();
            return mapped;
        }

        public async Task<KitchenResponseModel> GetSingleKitchenByKitchenId(int id)
        {
            var result = await _context.Kitchens
        .Include(x => x.User)
        .ThenInclude(x => x.Wallets)
        .Where(x => x.KitchenId == id)
        .Select(x => new KitchenResponseModel
        {
            KitchenId = x.KitchenId,
            Address = x.Address,
            Name = x.Name,
            DistrictDtoGetKitchen = new DistrictDtoGetKitchen
            {
                DistrictId = x.District.DistrictId,
                DistrictName = x.District.DistrictName,
            },
            UserDtoKitchenResponseModel = new UserDtoKitchenResponseModel
            {
                UserId = x.User.UserId,
                Email = x.User.Email,
                Phone = x.User.Phone,
                Username = x.User.Username,
                WalletDtoKitchenResponseModel = new WalletDtoKitchenResponseModel
                {
                    // Assuming Wallets is a collection and you want the balance from the first wallet
                    UserId = x.User.Wallets.FirstOrDefault().UserId,
                    Balance = x.User.Wallets.FirstOrDefault().Balance,
                    WalletId = x.User.Wallets.FirstOrDefault().WalletId,
                }
            }
        })
        .FirstOrDefaultAsync();

            var mapped = _mapper.Map<KitchenResponseModel>(result);
            return mapped;
        }

        public async Task<List<GetAllKitchenBySessionIdResponseModel>> GetAllKitchenBySessionId(int sessionid)
        {
            var kitchenid = _context.MealSessions
                .Where(x => x.SessionId == sessionid)
                .Select(x => x.KitchenId);

            var result = _context.Kitchens.Where(x => kitchenid.Contains(x.KitchenId)).Select(x => new GetAllKitchenBySessionIdResponseModel
            {
                KitchenId = x.KitchenId,
                Address = x.Address,
                Name = x.Name,
                UserId = (int)x.UserId,
                UserDtoGetAllKitchenBySessionId = new UserDtoGetAllKitchenBySessionId
                {
                    Name = x.User.Name,
                    UserId = x.User.UserId,
                    Username = x.User.Username,
                    Address = x.User.Address,
                    DistrictId = x.User.DistrictId,
                    Email = x.User.Email,
                    Phone = x.User.Phone,
                },
                AreaDtoGetAllKitchenBySessionId = new AreaDtoGetAllKitchenBySessionId
                {
                    Address = x.Area.Address,
                    AreaId = x.Area.AreaId,
                    AreaName = x.Area.AreaName,
                },
            });

            var mapped = result.Select(result => _mapper.Map<GetAllKitchenBySessionIdResponseModel>(result)).ToList();
            return mapped;
        }

        public async Task<KitchenResponseModel> GetSingleKitchenByUserId(int userid)
        {
            var result = await _context.Kitchens
        .Include(x => x.User)
        .ThenInclude(x => x.Wallets)
        .Where(x => x.UserId == userid)
        .Select(x => new KitchenResponseModel
        {
            KitchenId = x.KitchenId,
            Address = x.Address,
            Name = x.Name,
            DistrictDtoGetKitchen = new DistrictDtoGetKitchen
            {
                DistrictId = x.District.DistrictId,
                DistrictName = x.District.DistrictName,
            },
            UserDtoKitchenResponseModel = new UserDtoKitchenResponseModel
            {
                UserId = x.User.UserId,
                Email = x.User.Email,
                Phone = x.User.Phone,
                Username = x.User.Username,
                WalletDtoKitchenResponseModel = new WalletDtoKitchenResponseModel
                {
                    // Assuming Wallets is a collection and you want the balance from the first wallet
                    UserId = x.User.Wallets.FirstOrDefault().UserId,
                    Balance = x.User.Wallets.FirstOrDefault().Balance,
                    WalletId = x.User.Wallets.FirstOrDefault().WalletId,
                }
            },
            AreaDtoGetKitchen = new AreaDtoGetKitchen
            {
                AreaId = x.Area.AreaId,
                Address = x.Area.Address,
                AreaName = x.Area.AreaName,
                DistrictId = x.Area.DistrictId,
            }

        })
        .FirstOrDefaultAsync();

            var mapped = _mapper.Map<KitchenResponseModel>(result);
            return mapped;
        }

        public async Task<List<GetAllKitchenByAreaIdResponseModel>> GetAllKitchenByAreaId(int areaId)
        {

            var result = _context.Kitchens.Include(x => x.MealSessions).Where(x => x.AreaId == areaId && x.MealSessions.Any()).Select(x => new GetAllKitchenByAreaIdResponseModel
            {
                KitchenId = x.KitchenId,
                Address = x.Address,
                DistrictId = x.DistrictId,
                Name = x.Name,
                UserId = x.UserId,
                AreaDtoGetAllKitchenByAreaId = new AreaDtoGetAllKitchenByAreaId
                {
                    AreaId = x.Area.AreaId,
                    AreaName = x.Area.AreaName,
                    Address = x.Area.Address,
                },

                MealSessionDtoGetAllKitchenByAreaId = x.MealSessions
                .Select(ms => new MealSessionDtoGetAllKitchenByAreaId
                {
                    MealSessionId = ms.MealSessionId,
                    MealId = ms.MealId,
                    CreateDate = ((DateTime)ms.CreateDate).ToString("dd-MM-yyyy"),
                    KitchenId = ms.KitchenId,
                    Price = ms.Price,
                    Quantity = ms.Quantity,
                    RemainQuantity = ms.RemainQuantity,
                    SessionId = ms.SessionId,
                    Status = ms.Status,
                })
                .ToList()
            }).ToList();

            var mapped = result.Select(r => _mapper.Map<GetAllKitchenByAreaIdResponseModel>(r)).ToList();
            return mapped;

        }
    }

}
