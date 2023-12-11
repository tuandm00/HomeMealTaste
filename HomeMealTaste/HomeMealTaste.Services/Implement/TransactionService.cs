using AutoMapper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Services.Implement
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly HomeMealTasteContext _context;
        private readonly IMapper _mapper;
        private readonly IKitchenService _kitchenService;
        private readonly IOrderService _orderService;



        public TransactionService(ITransactionRepository transactionRepository, HomeMealTasteContext context, IMapper mapper, IKitchenService kitchenService, IOrderService orderService)
        {
            _transactionRepository = transactionRepository;
            _context = context;
            _mapper = mapper;
            _kitchenService = kitchenService;
            _orderService = orderService;
        }
        public static DateTime TranferDateTimeByTimeZone(DateTime dateTime, string timezoneArea)
        {

            ReadOnlyCollection<TimeZoneInfo> collection = TimeZoneInfo.GetSystemTimeZones();
            var timeZone = collection.ToList().Where(x => x.DisplayName.ToLower().Contains(timezoneArea)).First();

            var timeZoneLocal = TimeZoneInfo.Local;

            var utcDateTime = TimeZoneInfo.ConvertTime(dateTime, timeZoneLocal, timeZone);

            return utcDateTime;
        }

        public static DateTime GetDateTimeTimeZoneVietNam()
        {

            return TranferDateTimeByTimeZone(DateTime.Now, "hanoi");
        }
        public static DateTime? StringToDateTimeVN(string dateStr)
        {

            var isValid = System.DateTime.TryParseExact(
                                dateStr,
                                "d'/'M'/'yyyy",
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None,
                                out var date
                            );
            return isValid ? date : null;
        }

        public async Task<List<GetAllTransactionByTransactionTypeORDERED>> GetAllTransactionByTransactionTypeORDERED()
        {
            var result = _context.Transactions
                .Where(x => x.TransactionType == "ORDERED")
                .Select(t => new GetAllTransactionByTransactionTypeORDERED
                {
                    TransactionId = t.TransactionId,
                    OrderId = t.OrderId,
                    WalletDtoGetAllTransaction = new WalletDtoGetAllTransaction
                    {
                        WalletId = t.Wallet.WalletId,
                        UserDtoGetAllTransaction = new UserDtoGetAllTransaction
                        {
                            UserId = t.Wallet.User.UserId,
                            Name = t.Wallet.User.Name,
                            Username = t.Wallet.User.Username,
                            Email = t.Wallet.User.Email,
                            Phone = t.Wallet.User.Phone,
                            Address = t.Wallet.User.Address,
                            DistrictId = t.Wallet.User.DistrictId,
                            Status = t.Wallet.User.Status,
                            AreaId = t.Wallet.User.AreaId,
                        },
                        Balance = t.Wallet.Balance,
                    },
                    Date = ((DateTime)t.Date).ToString("dd-MM-yyyy HH:mm"),
                    Amount = t.Amount,
                    Description = t.Description,
                    Status = t.Status,
                    TransactionType = t.TransactionType,
                });
            var mapped = result.Select(result => _mapper.Map<GetAllTransactionByTransactionTypeORDERED>(result)).ToList();
            return mapped;
        }

        public async Task<List<GetAllTransactionByTransactionTypeRECHARGED>> GetAllTransactionByTransactionTypeRECHARGED()
        {
            var result = _context.Transactions
                .Where(x => x.TransactionType == "RECHARGED")
                .Select(t => new GetAllTransactionByTransactionTypeRECHARGED
                {
                    TransactionId = t.TransactionId,
                    OrderId = t.OrderId,
                    WalletDtoGetAllTransactionRECHARGED = new WalletDtoGetAllTransactionRECHARGED
                    {
                        WalletId = t.Wallet.WalletId,
                        UserDtoGetAllTransactionRECHARGED = new UserDtoGetAllTransactionRECHARGED
                        {
                            UserId = t.Wallet.User.UserId,
                            Name = t.Wallet.User.Name,
                            Username = t.Wallet.User.Username,
                            Email = t.Wallet.User.Email,
                            Phone = t.Wallet.User.Phone,
                            Address = t.Wallet.User.Address,
                            DistrictId = t.Wallet.User.DistrictId,
                            Status = t.Wallet.User.Status,
                            AreaId = t.Wallet.User.AreaId,
                        },
                        Balance = t.Wallet.Balance,
                    },
                    Date = t.Date.ToString(),
                    Amount = t.Amount,
                    Description = t.Description,
                    Status = t.Status,
                    TransactionType = t.TransactionType,
                });
            var mapped = result.Select(result => _mapper.Map<GetAllTransactionByTransactionTypeRECHARGED>(result)).ToList();
            return mapped;
        }

        public async Task<List<GetAllTransactionByUserIdResponseModel>> GetAllTransactionByUserId(int userid)
        {
            var userids = _context.Users.Where(u => u.UserId == userid).Select(u => u.UserId).FirstOrDefault();
            var result = _context.Transactions
        //.Include(x => x.Wallet)
        //.ThenInclude(x => x.User)
        //.Include(x => x.Order)
        .Where(x => x.UserId == userids).Select(x => new GetAllTransactionByUserIdResponseModel
        {
            TransactionId = x.TransactionId,
            UserId = x.UserId,
            Amount = x.Amount,
            Date = ((DateTime)x.Date).ToString("dd-MM-yyyy HH:mm"),
            Description = x.Description,
            Status = x.Status,
            TransactionType = x.TransactionType,
            OrderDtoTransactionResponse = new OrderDtoTransactionResponse
            {
                 OrderId = x.Order.OrderId,
                 CustomerDtoTransaction = new CustomerDtoTransaction
                 {
                     CustomerId = x.Order.Customer.CustomerId,
                     AreaId = x.Order.Customer.AreaId,
                     DistrictId = x.Order.Customer.DistrictId,
                     Name = x.Order.Customer.Name,
                     Phone = x.Order.Customer.Phone,
                     UserId = x.Order.Customer.UserId,
                 },
                 MealSessionId = x.Order.MealSessionId,
                 Quantity = x.Order.Quantity,
                 Time = ((DateTime)x.Date).ToString("HH:mm"),
                 TotalPrice = x.Order.TotalPrice,
                 Status = x.Order.Status,
            },
            WalletDtoTransactionResponse = new WalletDtoTransactionResponse
            {
                WalletId = x.Wallet.WalletId,
                Balance = x.Wallet.Balance,
                UserId = x.Wallet.UserId,
            }
        })
        .ToList();
        var mapped = result.Select(r => _mapper.Map<GetAllTransactionByUserIdResponseModel>(r)).ToList();
        return mapped;

        }

        public async Task<List<SaveTotalPriceAfterFinishSessionResponseModel>> SaveTotalPriceAfterFinishSession(int sessionId)
        {
            var getAllKitchenBySession = await _kitchenService.GetAllKitchenBySessionId(sessionId);
            var savedTransactions = new List<Transaction>();
            foreach (var i in getAllKitchenBySession)
            {
                var getTotal = _orderService.GetTotalPriceWithMealSessionBySessionIdAndKitchenId(sessionId, i.KitchenId);
                //var kitchen = await _context.Kitchens.Where(x => x.KitchenId == i.KitchenId).FirstOrDefaultAsync();
                var saveToTransaction = new Transaction
                {
                    OrderId = null,
                    WalletId = null,
                    Date = GetDateTimeTimeZoneVietNam(),
                    Amount = await getTotal,
                    Description = "MONEY TRANSFER TO CHEF: " + i.Name,
                    Status = "SUCCEED",
                    TransactionType = "TT",
                    UserId = i.UserId,
                };
                _context.Transactions.Add(saveToTransaction);
                savedTransactions.Add(saveToTransaction);
            }
            await _context.SaveChangesAsync();
            var mapped = savedTransactions.Select(transaction => _mapper.Map<SaveTotalPriceAfterFinishSessionResponseModel>(transaction)).ToList();
            return mapped;
        }

        public async Task<List<GetAllTransactionsResponseModel>> GetAllTransaction()
        {
            var result = _context.Transactions.Select(x => new GetAllTransactionsResponseModel
            {
                TransactionId = x.TransactionId,
                Amount = x.Amount,
                Date = ((DateTime)x.Date).ToString("dd-MM-yyyy"),
                Description = x.Description,
                Status = x.Status,
                TransactionType = x.TransactionType,
                OrderDtoGetAllTransactions = new OrderDtoGetAllTransactions
                {
                    OrderId = x.Order.OrderId,
                    Status = x.Order.Status,
                    CustomerId = x.Order.CustomerId,
                    MealSessionId = x.Order.MealSessionId,
                    Quantity = x.Order.Quantity,
                    Time = ((DateTime)x.Order.Time).ToString("HH:mm"),
                    TotalPrice = x.Order.TotalPrice,
                },
                UserDtoGetAllTransactions = new UserDtoGetAllTransactions
                {
                    UserId = x.User.UserId,
                    Name = x.User.Name,
                    Address = x.User.Address,
                    AreaId = x.User.AreaId,
                    DistrictId = x.User.DistrictId,
                    Email = x.User.Email,
                    Phone = x.User.Phone,
                    Username = x.User.Username,
                    RoleId = x.User.RoleId,
                },
                WalletDtoGetAllTransactions = new WalletDtoGetAllTransactions
                {
                    Balance = x.Wallet.Balance,
                    UserId = x.Wallet.UserId,
                    WalletId = x.Wallet.WalletId,
                },
            }).ToList();

            var mapped = result.Select(r => _mapper.Map<GetAllTransactionsResponseModel>(r)).ToList();
            return mapped;
        }

        
    }
}
