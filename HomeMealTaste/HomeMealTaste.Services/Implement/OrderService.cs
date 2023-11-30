﻿using AutoMapper;
using HomeMealTaste.Data.Helper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Security.Claims;

namespace HomeMealTaste.Services.Implement
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly HomeMealTasteContext _context;
        public OrderService(IOrderRepository orderRepository, IMapper mapper, HomeMealTasteContext context)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _context = context;
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

        public async Task<List<OrderResponseModel>> GetAllOrder()
        {
            var result = _context.Orders
                .Include(x => x.MealSession.Meal.Kitchen)
                .Include(x => x.MealSession)
                .Select(x => new OrderResponseModel
                {
                    OrderId = x.OrderId,
                    Time = ((DateTime)x.Time).ToString("HH:mm"),
                    Quantity = x.Quantity,

                    CustomerDto1 = new CustomerDto1
                    {
                        CustomerId = x.Customer.CustomerId,
                        Name = x.Customer.Name,
                        Phone = x.Customer.Phone,
                        DistrictId = x.Customer.DistrictId,
                        AreaId = x.Customer.AreaId,
                        UserId = x.Customer.UserId,
                    },
                    MealSessionDto1 = new MealSessionDto1
                    {
                        MealSessionId = x.MealSession.MealSessionId,
                        MealDto1 = new MealDto1
                        {
                            MealId = x.MealSession.Meal.MealId,
                            Name = x.MealSession.Meal.Name,
                            Image = x.MealSession.Meal.Image,
                            KitchenDto1 = new KitchenDto1
                            {
                                KitchenId = x.MealSession.Meal.Kitchen.KitchenId,
                                UserId = x.MealSession.Meal.Kitchen.UserId,
                                Name = x.MealSession.Meal.Kitchen.Name,
                                Address = x.MealSession.Meal.Kitchen.Address,
                                AreaId = x.MealSession.Meal.Kitchen.AreaId,
                            },
                            CreateDate = GetDateTimeTimeZoneVietNam().ToString("dd-MM-yyyy"),
                            Description = x.MealSession.Meal.Description,
                        },
                        SessionDto1 = new SessionDto1
                        {
                            SessionId = x.MealSession.Session.SessionId,
                            CreateDate = ((DateTime)x.MealSession.Session.CreateDate).ToString("dd-MM-yyyy"),
                            StartTime = ((DateTime)x.MealSession.Session.StartTime).ToString("HH:mm"),
                            EndTime = ((DateTime)x.MealSession.Session.EndTime).ToString("HH:mm"),
                            EndDate = ((DateTime)x.MealSession.Session.EndDate).ToString("dd-MM-yyyy"),
                            UserId = x.MealSession.Session.UserId,
                            Status = x.MealSession.Session.Status,
                            SessionType = x.MealSession.Session.SessionType,
                            AreaId = x.MealSession.Session.AreaId,
                        },
                        Price = x.MealSession.Price,
                        Quantity = x.MealSession.Quantity,
                        RemainQuantity = x.MealSession.RemainQuantity,
                        Status = x.MealSession.Status,
                        CreateDate = GetDateTimeTimeZoneVietNam().ToString("dd-MM-yyyy"),
                    },
                    Status = x.Status,
                    Price = x.TotalPrice,
                });
            var mappedResult = result.Select(x => _mapper.Map<OrderResponseModel>(x)).ToList();

            return mappedResult;
        }

        public Task<GetAllOrderByUserIdResponseModel> GetSingleOrderById(int id)
        {
            var results = _context.Orders
                .Include(x => x.MealSession.Meal.Kitchen)
                .Include(x => x.MealSession)
                .Where(x => x.OrderId == id).Select(x => new GetAllOrderByUserIdResponseModel
                {
                    OrderId = x.OrderId,
                    Time = ((DateTime)x.Time).ToString("HH:mm"),
                    CustomerDto2 = new CustomerDto2
                    {
                        CustomerId = x.Customer.CustomerId,
                        Name = x.Customer.Name,
                        Phone = x.Customer.Phone,
                        DistrictId = x.Customer.DistrictId,
                        AreaId = x.Customer.AreaId,
                        UserId = x.Customer.UserId,
                    },
                    MealSessionDto2 = new MealSessionDto2
                    {
                        MealSessionId = x.MealSession.MealSessionId,
                        MealDto2 = new MealDto2
                        {
                            MealId = x.MealSession.Meal.MealId,
                            Name = x.MealSession.Meal.Name,
                            Image = x.MealSession.Meal.Image,
                            KitchenDto2 = new KitchenDto2
                            {
                                KitchenId = x.MealSession.Meal.Kitchen.KitchenId,
                                UserId = x.MealSession.Meal.Kitchen.UserId,
                                Name = x.MealSession.Meal.Kitchen.Name,
                                Address = x.MealSession.Meal.Kitchen.Address,
                                AreaId = x.MealSession.Meal.Kitchen.AreaId,
                            },
                            CreateDate = GetDateTimeTimeZoneVietNam().ToString("dd-MM-yyyy"),
                            Description = x.MealSession.Meal.Description,

                        },
                        SessionDto2 = new SessionDto2
                        {
                            SessionId = x.MealSession.Session.SessionId,
                            CreateDate = ((DateTime)x.MealSession.Session.CreateDate).ToString("dd-MM-yyyy"),
                            StartTime = ((DateTime)x.MealSession.Session.StartTime).ToString("HH:mm"),
                            EndTime = ((DateTime)x.MealSession.Session.EndTime).ToString("HH:mm"),
                            EndDate = ((DateTime)x.MealSession.Session.EndDate).ToString("dd-MM-yyyy"),
                            UserId = x.MealSession.Session.UserId,
                            Status = x.MealSession.Session.Status,
                            SessionType = x.MealSession.Session.SessionType,
                            AreaId = x.MealSession.Session.AreaId,
                        },
                        Price = x.MealSession.Price,
                        Quantity = x.MealSession.Quantity,
                        RemainQuantity = x.MealSession.RemainQuantity,
                        Status = x.MealSession.Status,
                        CreateDate = GetDateTimeTimeZoneVietNam().ToString("dd-MM-yyyy"),
                    },
                    Status = x.Status,
                    TotalPrice = x.TotalPrice,
                    Quantity = x.Quantity,
                }).FirstOrDefault();

            return Task.FromResult(results);
        }

        public async Task<List<GetAllOrderByUserIdResponseModel>> GetAllOrderByCustomerId(int id)
        {
            var results = _context.Orders.Include(x => x.MealSession).Where(x => x.CustomerId == id).Select(x => new GetAllOrderByUserIdResponseModel
            {
                OrderId = x.OrderId,
                Time = ((DateTime)x.Time).ToString("HH:mm"),

                CustomerDto2 = new CustomerDto2
                {
                    CustomerId = x.Customer.CustomerId,
                    Name = x.Customer.Name,
                    Phone = x.Customer.Phone,
                    DistrictId = x.Customer.DistrictId,
                    AreaId = x.Customer.AreaId,
                    UserId = x.Customer.UserId
                },
                MealSessionDto2 = new MealSessionDto2
                {
                    MealSessionId = x.MealSession.MealSessionId,
                    MealDto2 = new MealDto2
                    {
                        MealId = x.MealSession.Meal.MealId,
                        Name = x.MealSession.Meal.Name,
                        Image = x.MealSession.Meal.Image,
                        KitchenDto2 = new KitchenDto2
                        {
                            KitchenId = x.MealSession.Meal.Kitchen.KitchenId,
                            UserId = x.MealSession.Meal.Kitchen.UserId,
                            Name = x.MealSession.Meal.Kitchen.Name,
                            Address = x.MealSession.Meal.Kitchen.Address,
                            AreaId = x.MealSession.Meal.Kitchen.AreaId,
                        },
                        CreateDate = GetDateTimeTimeZoneVietNam().ToString("dd-MM-yyyy"),
                        Description = x.MealSession.Meal.Description,

                    },
                    SessionDto2 = new SessionDto2
                    {
                        SessionId = x.MealSession.Session.SessionId,
                        CreateDate = ((DateTime)x.MealSession.Session.CreateDate).ToString("dd-MM-yyyy"),
                        StartTime = ((DateTime)x.MealSession.Session.StartTime).ToString("HH:mm"),
                        EndTime = ((DateTime)x.MealSession.Session.EndTime).ToString("HH:mm"),
                        EndDate = ((DateTime)x.MealSession.Session.EndDate).ToString("dd-MM-yyyy"),
                        UserId = x.MealSession.Session.UserId,
                        Status = x.MealSession.Session.Status,
                        SessionType = x.MealSession.Session.SessionType,
                        AreaId = x.MealSession.Session.AreaId,
                    },
                    Price = x.MealSession.Price,
                    Quantity = x.MealSession.Quantity,
                    RemainQuantity = x.MealSession.RemainQuantity,
                    Status = x.MealSession.Status,
                    CreateDate = GetDateTimeTimeZoneVietNam().ToString("dd-MM-yyyy"),
                },
                Status = x.Status,
                TotalPrice = x.TotalPrice,
                Quantity = x.Quantity,

            }).ToList();

            var mappedResults = results.Select(order => _mapper.Map<GetAllOrderByUserIdResponseModel>(order)).ToList();
            return mappedResults;
        }

        public Task<List<GetOrderByKitchenIdResponseModel>> GetOrderByKitchenId(int kitchenid)
        {
            var result = _context.Orders
                .Include(x => x.MealSession)
                .ThenInclude(x => x.Meal)
                .ThenInclude(x => x.Kitchen)
                .Where(x => x.MealSession.Meal.Kitchen.KitchenId == kitchenid)
                .Select(x => new GetOrderByKitchenIdResponseModel
                {
                    OrderId = x.OrderId,
                    Time = ((DateTime)x.Time).ToString("HH:mm"),
                    Date = GetDateTimeTimeZoneVietNam().ToString(),
                    Customer = new CustomerDto
                    {
                        CustomerId = x.Customer.CustomerId,
                        UserId = x.Customer.UserId,
                        Name = x.Customer.Name,
                        Phone = x.Customer.Phone,
                        DistrictId = x.Customer.DistrictId,
                    },
                    MealSession = new MealSessionDto
                    {
                        MealSessionId = x.MealSession.MealSessionId,
                        MealDtoOrderResponse = new MealDtoOrderResponse
                        {
                            MealId = x.MealSession.Meal.MealId,
                            Name = x.MealSession.Meal.Name,
                            Image = x.MealSession.Meal.Image,
                            KitchenId = x.MealSession.Meal.KitchenId,
                            CreateDate = x.MealSession.Meal.CreateDate.ToString(),
                            Description = x.MealSession.Meal.Description
                        },
                        SessionDto = new SessionDto
                        {
                            SessionId = x.MealSession.Session.SessionId,
                            CreateDate = ((DateTime)x.MealSession.Session.CreateDate).ToString("dd-MM-yyyy"),
                            StartTime = ((DateTime)x.MealSession.Session.StartTime).ToString("HH:mm"),
                            EndTime = ((DateTime)x.MealSession.Session.EndTime).ToString("HH:mm"),
                            EndDate = ((DateTime)x.MealSession.Session.EndDate).ToString("dd-MM-yyyy"),
                            Status = x.MealSession.Session.Status,
                            SessionType = x.MealSession.Session.SessionType,
                            UserId = x.MealSession.Session.UserId,
                            AreaDtoOrderResponse = new AreaDtoOrderResponse
                            {
                                AreaId = x.MealSession.Session.Area.AreaId,
                                Address = x.MealSession.Session.Area.Address,
                                AreaName = x.MealSession.Session.Area.AreaName,
                                DistrictDtoOrderResponse = new DistrictDtoOrderResponse
                                {
                                    DistrictId = x.MealSession.Session.Area.District.DistrictId,
                                    DistrictName = x.MealSession.Session.Area.District.DistrictName,
                                }
                            }
                        },
                        Quantity = x.MealSession.Quantity,
                        RemainQuantity = x.MealSession.RemainQuantity,
                        Status = x.MealSession.Status,
                        CreateDate = ((DateTime)x.MealSession.CreateDate).ToString("dd-MM-yyyy"),
                        Price = (int?)x.MealSession.Price,
                    },
                    Status = x.Status,
                    TotalPrice = x.TotalPrice,
                });

            var mapped = result.Select(x => _mapper.Map<GetOrderByKitchenIdResponseModel>(x)).ToList();
            return Task.FromResult(mapped);


        }

        public async Task<CreateOrderResponseModel> CreateOrder(CreateOrderRequestModel createOrderRequest)
        {
            using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? transaction = _context.Database.BeginTransaction();
            var entity = _mapper.Map<Order>(createOrderRequest);
            var customerid = _context.Customers.Where(customerid => customerid.UserId == entity.CustomerId).FirstOrDefault();
            var mealsessionid = _context.MealSessions
                .Where(mealsession => mealsession.MealSessionId == entity.MealSessionId)
                .Include(mealsession => mealsession.Meal)
                    .ThenInclude(meal => meal.MealDishes)
                    .ThenInclude(mealDish => mealDish.Dish)
                .AsNoTracking().FirstOrDefault();

            var walletid = _context.Wallets
                .Include(user => user.User)
                .ThenInclude(customer => customer.Customers)
                .Where(x => x.UserId == x.User.UserId).FirstOrDefault();

            var price = mealsessionid.Price;
            var remainquantity = mealsessionid.RemainQuantity;
            mealsessionid.RemainQuantity = remainquantity - createOrderRequest.Quantity;
            var totalprice = price * createOrderRequest.Quantity;
            //add order to table order
            var createOrder = new CreateOrderRequestModel
            {

                CustomerId = entity.CustomerId,
                TotalPrice = (int?)totalprice,
                Time = GetDateTimeTimeZoneVietNam(),
                Status = "PAID",
                MealSessionId = mealsessionid.MealSessionId,
                Quantity = createOrderRequest.Quantity,
            };

            var customer = _context.Customers.Where(z => z.CustomerId == createOrder.CustomerId).FirstOrDefault();
            var user = _context.Users.Where(x => x.UserId == customer.UserId).FirstOrDefault();
            var walletCustomer = _context.Wallets.Where(x => x.UserId == user.UserId).FirstOrDefault();
            if (walletCustomer != null)
            {
                var afterBalanceCustomer = (int?)(walletCustomer.Balance - totalprice);

                if (walletCustomer != null)
                {
                    walletCustomer.Balance = afterBalanceCustomer;
                    _context.Wallets.Update(walletCustomer);
                }
            }
            // save to admin wallet take 10%
            var admin = _context.Users.Where(x => x.RoleId == 1).FirstOrDefault();
            if (admin != null)
            {
                var priceToAdmin = ((totalprice * 10) / 100);

                // Check if the admin already has a wallet
                var adminWallet = _context.Wallets.FirstOrDefault(w => w.UserId == admin.UserId);

                if (adminWallet != null)
                {
                    // Update the existing wallet
                    adminWallet.Balance += (int?)priceToAdmin;
                    _context.Wallets.Update(adminWallet);
                }
                else
                {
                    // Create a new wallet for the admin
                    var newWalletAdmin = new Wallet
                    {
                        UserId = admin.UserId,
                        Balance = (int?)priceToAdmin
                    };
                    _context.Wallets.Add(newWalletAdmin);
                }
            }

            //then transfer price after 10 % to kitchen
            var kitchen = _context.MealSessions
                .Where(x => x.MealSessionId == entity.MealSessionId)
                .Include(x => x.Kitchen)
                .AsNoTracking()
                .FirstOrDefault();

            if (kitchen != null)
            {
                var priceToKitchen = totalprice - ((totalprice * 10) / 100);
                var chefWallet = _context.Wallets.FirstOrDefault(w => w.UserId == kitchen.Kitchen.UserId);

                if (chefWallet != null)
                {
                    // Update the existing wallet
                    chefWallet.Balance += (int?)priceToKitchen;
                    _context.Wallets.Update(chefWallet);
                }
                else
                {
                    // Create a new wallet for the chef
                    var newWalletChef = new Wallet
                    {
                        UserId = kitchen.Kitchen.UserId,
                        Balance = (int?)priceToKitchen
                    };
                    _context.Wallets.Add(newWalletChef);
                }
            }

            _context.MealSessions.Update(mealsessionid);
            _context.Wallets.Update(walletid);

            var orderEntity = _mapper.Map<Order>(createOrder);
            await _context.AddAsync(orderEntity);
            await _context.SaveChangesAsync();
            var useridwallet = _context.Wallets.Select(x => x.UserId).FirstOrDefault();
            //save to table transaction
            var transactionid = new Transaction
            {
                OrderId = orderEntity.OrderId,
                WalletId = walletid.WalletId,
                Date = createOrder.Time,
                Amount = (decimal?)totalprice,
                Description = "DONE WITH PAYMENT",
                Status = "SUCCEED",
                TransactionType = "ORDERED",
                UserId = user.UserId,
            };

            _context.Transactions.Add(transactionid);

            await _context.SaveChangesAsync();
            transaction.Commit();
            var mapped = _mapper.Map<CreateOrderResponseModel>(orderEntity);
            return mapped;
        }

        public async Task<RefundMoneyToWalletByOrderIdResponseModel> RefundMoneyToCustomer(RefundMoneyToWalletByOrderIdRequestModel refundRequest)
        {
            using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? transaction = _context.Database.BeginTransaction();
            var entity = _mapper.Map<Order>(refundRequest);
            var orderid = _context.Orders.Where(x => x.OrderId == entity.OrderId).FirstOrDefault();
            if (orderid != null && orderid.Status.Equals("PAID"))
            {
                orderid.Status = "CANCELLED";
                _context.Orders.Update(orderid);
                _context.SaveChanges();
            }

            var totalPriceOfOrder = orderid.TotalPrice;
            var customerIdOfOrder = orderid.CustomerId;
            var userId = _context.Customers.Where(x => x.CustomerId == customerIdOfOrder).Select(x => x.UserId).FirstOrDefault();
            var walletOfCustomer = _context.Wallets.Where(x => x.UserId == userId).FirstOrDefault();
            var balanceExisted = walletOfCustomer.Balance;
            var newBalanceForCustomer = balanceExisted + ((totalPriceOfOrder * 90) / 100);

            // customer receive 90% money back
            if (walletOfCustomer != null)
            {
                walletOfCustomer.Balance = newBalanceForCustomer;
                _context.Wallets.Update(walletOfCustomer);
            }

            // admin keep 10% totalPrice
            var admin = _context.Users.Where(x => x.RoleId == 1).FirstOrDefault();
            if (admin != null)
            {
                var newBalanceForAdmin = ((totalPriceOfOrder * 10) / 100);

                var adminWallet = _context.Wallets.FirstOrDefault(w => w.UserId == admin.UserId);

                if (adminWallet != null)
                {
                    adminWallet.Balance += (int?)newBalanceForAdmin;
                    _context.Wallets.Update(adminWallet);
                }
            }
            // back a remainquantity
            var remainquantityInMealSession = _context.MealSessions
                .Where(x => x.MealSessionId == orderid.MealSessionId)
                .Select(x => x.RemainQuantity).FirstOrDefault();
            if (remainquantityInMealSession != null)
            {
                var newRemainQuantity = remainquantityInMealSession + orderid.Quantity;
                var mealSession = _context.MealSessions.FirstOrDefault(x => x.MealSessionId == orderid.MealSessionId);
                if (mealSession != null)
                {
                    mealSession.RemainQuantity = newRemainQuantity;
                    _context.MealSessions.Update(mealSession);
                }
            }
            // minus money of kitchen in wallet as 90% totalPrice
            var kitchenid = _context.MealSessions.Where(x => x.MealSessionId == orderid.MealSessionId).Select(x => x.KitchenId).FirstOrDefault();
            var userIdOfKitchen = _context.Kitchens.Where(x => x.KitchenId == kitchenid).Select(x => x.UserId).FirstOrDefault();
            var kitchenWallet = _context.Wallets.Where(x => x.UserId == userIdOfKitchen).FirstOrDefault();
            if (kitchenWallet != null)
            {
                kitchenWallet.Balance -= ((totalPriceOfOrder * 90) / 100);
                _context.Wallets.Update(kitchenWallet);
            }
            _context.SaveChanges();
            transaction.Commit();
            var mapped = _mapper.Map<RefundMoneyToWalletByOrderIdResponseModel>(orderid);
            return mapped;
        }

        public async Task<ChangeStatusOrderToCompletedResponseModel> ChangeStatusOrderToCompleted(int orderid)
        {
            var result = await _context.Orders.Where(x => x.OrderId == orderid).FirstOrDefaultAsync();
            if(result != null && result.Status.Equals("PAID", StringComparison.OrdinalIgnoreCase))
            {
                result.Status = "COMPLETED";
                await _context.SaveChangesAsync();

                return new ChangeStatusOrderToCompletedResponseModel
                {
                    OrderId = orderid,
                    Status = result.Status,
                    CustomerId = result.CustomerId,
                    MealSessionId = result.MealSessionId,
                    TotalPrice = result.TotalPrice,
                    Time = result.Time.ToString(),
                    Quantity = result.Quantity,
                };
            }
            return null;
        }
    }
}

