﻿using AutoMapper;
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
                    Date = t.Date.ToString(),
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
            var result = _context.Transactions
        .Include(x => x.Wallet)
        .ThenInclude(x => x.User)
        .Include(x => x.Order)
        .Where(x => x.Wallet.UserId == userid)
        .ToList();

            var mapped = result.Select(transaction =>
            {
                var response = _mapper.Map<GetAllTransactionByUserIdResponseModel>(transaction);
                response.TransactionId = transaction.TransactionId;
                response.Date = transaction.Date.ToString();
                response.Amount = transaction.Amount;
                response.Description = transaction.Description;
                response.Status = transaction.Status;
                response.OrderDtoTransactionResponse = new OrderDtoTransactionResponse
                {
                    OrderId = transaction.Order.OrderId,
                    CustomerId = transaction.Order.CustomerId,
                    Status = transaction.Order.Status,
                    MealSessionId = transaction.Order.MealSessionId,
                    TotalPrice = transaction.Order.TotalPrice,
                    Time = transaction.Order.Time.ToString(),
                    Quantity = transaction.Order.Quantity,
                };
                response.WalletDtoTransactionResponse = new WalletDtoTransactionResponse
                {
                    WalletId = transaction.Wallet.WalletId,
                    UserId = transaction.Wallet.UserId,
                    Balance = transaction.Wallet.Balance,
                };
                return response;
            }).ToList();

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
                    TransactionType = "TOTAL TRANSFER",
                    UserId = i.UserId,
                };
                _context.Transactions.Add(saveToTransaction);
                savedTransactions.Add(saveToTransaction);
            }
            await _context.SaveChangesAsync();
            var mapped = savedTransactions.Select(transaction => _mapper.Map<SaveTotalPriceAfterFinishSessionResponseModel>(transaction)).ToList();
            return mapped;
        }
    }
}
