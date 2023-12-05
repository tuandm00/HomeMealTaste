using AutoMapper;
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
                    Time = ((DateTime)x.Time).ToString("dd-MM-yyy HH:mm"),
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
                    Time = ((DateTime)x.Time).ToString("dd-MM-yyyy HH:mm"),
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
                            CreateDate = ((DateTime)x.MealSession.Meal.CreateDate).ToString("dd-MM-yyyy"),
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
            var mealSessionIdInOrder = _context.Orders.Select(x => x.MealSessionId).ToList();
            foreach (var id in mealSessionIdInOrder)
            {
                if (entity.MealSessionId != id)
                {
                    var sessionCheck1 = _context.MealSessions.Where(x => x.MealSessionId == entity.MealSessionId).Select(x => x.SessionId).FirstOrDefault();
                    var sessionCheck2 = _context.MealSessions.Where(x => x.MealSessionId == id).Select(x => x.SessionId).FirstOrDefault();
                    if (sessionCheck1 == sessionCheck2)
                    {
                        throw new Exception("Can Not Order In Same Session");
                    }
                }
            }
            var customerid = _context.Customers.Where(customerid => customerid.UserId == entity.CustomerId).FirstOrDefault();
            var mealsessionid = _context.MealSessions
                .Where(mealsession => mealsession.MealSessionId == entity.MealSessionId && mealsession.Status.Equals("APPROVED"))
                .Include(mealsession => mealsession.Meal)
                    .ThenInclude(meal => meal.MealDishes)
                    .ThenInclude(mealDish => mealDish.Dish)
                .AsNoTracking().FirstOrDefault();

            var walletid = _context.Wallets
                .Include(user => user.User)
                .ThenInclude(customer => customer.Customers)
                .Where(x => x.UserId == x.User.UserId).FirstOrDefault();

            if (mealsessionid == null)
            {
                throw new Exception("Session is not start");
            }
            if (mealsessionid.RemainQuantity == 0)
            {
                throw new Exception("No meal can order because the quantity is over");
            }
            var price = mealsessionid.Price;
            var remainquantity = mealsessionid.RemainQuantity;
            mealsessionid.RemainQuantity = remainquantity - createOrderRequest.Quantity;
            var totalprice = price * createOrderRequest.Quantity;
            //check mealsessionid then add order to table order
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

                if (afterBalanceCustomer >= 0)
                {
                    walletCustomer.Balance = afterBalanceCustomer;
                    _context.Wallets.Update(walletCustomer);
                }
                else
                {
                    throw new Exception("YOUR BALANCE IS OUT OF AMOUNT");
                }
            }
            // save to admin wallet take 10%
            var admin = _context.Users.Where(x => x.RoleId == 1).FirstOrDefault();
            var priceToAdmin = (totalprice * 10) / 100;

            if (admin != null)
            {
                // Check if the admin already has a wallet
                var adminWallet = _context.Wallets.FirstOrDefault(w => w.UserId == admin.UserId);

                if (adminWallet != null)
                {
                    // Update the existing wallet
                    adminWallet.Balance += (int?)priceToAdmin;
                    _context.Wallets.Update(adminWallet);
                }

            }

            //then transfer price after 10 % of admin to kitchen
            var kitchen = _context.MealSessions
                .Where(x => x.MealSessionId == entity.MealSessionId)
                .Include(x => x.Kitchen)
                .AsNoTracking()
                .FirstOrDefault();
            var priceToChef = totalprice - priceToAdmin;

            if (kitchen != null)
            {
                var chefWallet = _context.Wallets.FirstOrDefault(w => w.UserId == kitchen.Kitchen.UserId);

                if (chefWallet != null)
                {
                    // Update the existing wallet
                    chefWallet.Balance += (int?)priceToChef;
                    _context.Wallets.Update(chefWallet);
                }

            }

            _context.MealSessions.Update(mealsessionid);
            _context.Wallets.Update(walletid);

            var orderEntity = _mapper.Map<Order>(createOrder);
            await _context.AddAsync(orderEntity);
            await _context.SaveChangesAsync();

            // Save to transaction for admin
            var transactionToAdmin = new Transaction
            {
                OrderId = orderEntity.OrderId,
                WalletId = walletid.WalletId,
                Date = createOrder.Time,
                Amount = (decimal?)priceToAdmin,
                Description = "DONE WITH REVENUE",
                Status = "SUCCEED",
                TransactionType = "RR",
                UserId = admin.UserId,
            };

            _context.Transactions.Add(transactionToAdmin);

            // Save to transaction for chef
            var transactionToChef = new Transaction
            {
                OrderId = orderEntity.OrderId,
                WalletId = walletid.WalletId,
                Date = createOrder.Time,
                Amount = (decimal?)priceToChef,
                Description = "DONE WITH REVENUE",
                Status = "SUCCEED",
                TransactionType = "RR",
                UserId = kitchen.Kitchen.UserId,
            };

            _context.Transactions.Add(transactionToChef);


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

        public async Task ChefCancelledOrderRefundMoneyToCustomer(RefundMoneyToWalletByOrderIdRequestModel refundRequest)
        {
            using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? transaction = _context.Database.BeginTransaction();
            var entity = _mapper.Map<Transaction>(refundRequest);

            var orderIdinTransaction = await _context.Transactions
                .Where(x => x.OrderId == entity.OrderId)
                .ToListAsync();

            var orderIdsInTransaction = orderIdinTransaction.Select(t => t.OrderId).ToList();

            var ordersInOrder = await _context.Orders
                .Where(x => orderIdsInTransaction.Contains(x.OrderId))
                .ToListAsync();

            if (ordersInOrder.Count == 0)
            {
                throw new Exception("Cannot find this Order, please try again");
            }

            foreach (var order in ordersInOrder)
            {
                order.Status = "CANCELLED";
            }

            //get userId of Kitchen in Transaction throw Order -> MealSession -> KitchenId -> UserId
            //get userId of Customer in Transaction throw Order -> Customer -> UserId
            var mealsessionId = await _context.Orders
                .Where(x => x.OrderId == entity.OrderId)
                .Select(x => x.MealSessionId)
                .FirstOrDefaultAsync(); // get mealsessionid by checked orderid in table Order == OrderId input


            var kitchenIdOfMealSession = await _context.MealSessions
                .Where(x => x.MealSessionId == mealsessionId)
                .Select(x => x.KitchenId)
                .FirstOrDefaultAsync(); // get KitchenId in Mealsession 


            var userIdOfKitchen = await _context.Kitchens
                .Where(x => x.KitchenId == kitchenIdOfMealSession)
                .Select(x => x.UserId)
                .FirstOrDefaultAsync(); // get userId by KitchenId in Mealsession 

            var amountOfKitchenByUserId = await _context.Transactions
                .Where(x => x.UserId == userIdOfKitchen)
                .Select(x => x.Amount)
                .FirstOrDefaultAsync(); // get amount of kitchenid == userid

            var customerid = await _context.Orders
                .Where(x => x.OrderId == entity.OrderId)
                .Select(x => x.CustomerId)
                .FirstOrDefaultAsync(); // get customerid throw order

            var userIdOfCustomer = await _context.Customers
                .Where(x => x.CustomerId == customerid)
                .Select(x => x.UserId)
                .FirstOrDefaultAsync();// get userid in table Customer

            var walletOfUserIdOfKitchen = await _context.Wallets
                .Where(x => x.UserId == userIdOfKitchen)
                .Select(x => x.Balance)
                .FirstOrDefaultAsync(); // get wallet of userId of kitchen

            var walletOfUserIdOfCustomer = await _context.Wallets
                .Where(x => x.UserId == userIdOfCustomer)
                .Select(x => x.Balance)
                .FirstOrDefaultAsync();

            var walletIdOfUserIdOfKitchen = await _context.Wallets
                .Where(x => x.UserId == userIdOfKitchen)
                .Select(x => x.WalletId)
                .FirstOrDefaultAsync(); // get walletid by userid
            
            var walletIdOfUserIdOfCustomer = await _context.Wallets
                .Where(x => x.UserId == userIdOfCustomer)
                .Select(x => x.WalletId)
                .FirstOrDefaultAsync(); // get walletid by userid

            var totalPrice = await _context.Orders
                .Where(x => x.OrderId == entity.OrderId)
                .Select(x => x.TotalPrice)
                .FirstOrDefaultAsync();

            var datenow = GetDateTimeTimeZoneVietNam();

            if (walletOfUserIdOfKitchen != null)
            {
                var afterCancelled = walletOfUserIdOfKitchen - totalPrice;
                var walletOfUserIdOfKitchenWithOutSelect = await _context.Wallets
                    .Where(x => x.UserId == userIdOfKitchen)
                    .FirstOrDefaultAsync();
                if (walletOfUserIdOfKitchenWithOutSelect != null)
                {
                    walletOfUserIdOfKitchenWithOutSelect.Balance = afterCancelled;
                    _context.Wallets.Update(walletOfUserIdOfKitchenWithOutSelect);


                    var addToTransactionChef = new Transaction
                    {
                        OrderId = entity.OrderId,
                        WalletId = walletIdOfUserIdOfKitchen,
                        Date = datenow,
                        Amount = totalPrice,
                        Description = "DONE WITH REFUND",
                        Status = "SUCCEED",
                        TransactionType = "REFUNDED",
                        UserId = userIdOfKitchen,
                    };
                    _context.Transactions.Add(addToTransactionChef);
                }
            }

            if (walletOfUserIdOfCustomer != null)
            {
                var walletOfUserIdOfCustomerWithOutSelect = await _context.Wallets
                    .Where(x => x.UserId == userIdOfCustomer)
                    .FirstOrDefaultAsync();
                if (walletOfUserIdOfCustomerWithOutSelect != null)
                {
                    walletOfUserIdOfCustomerWithOutSelect.Balance += totalPrice;
                    _context.Wallets.Update(walletOfUserIdOfCustomerWithOutSelect);

                    var addToTransactionCus = new Transaction
                    {
                        OrderId = entity.OrderId,
                        WalletId = walletIdOfUserIdOfCustomer,
                        Date = datenow,
                        Amount = totalPrice,
                        Description = "DONE WITH REFUND",
                        Status = "SUCCEED",
                        TransactionType = "REFUNDED",
                        UserId = userIdOfCustomer,
                    };
                    _context.Transactions.Add(addToTransactionCus);
                }
            }

            await _context.SaveChangesAsync();
            transaction.Commit();
        }

        public async Task<List<ChangeStatusOrderToCompletedResponseModel>> ChangeStatusOrderToDONE(int mealsessionid, string status)
        {
            var listOrder = await _context.Orders.Where(x => x.MealSessionId == mealsessionid).ToListAsync();
            
            if (listOrder != null) 
            { 

                foreach (var list in listOrder)
                {
                    if (status.Equals("DONE", StringComparison.OrdinalIgnoreCase) && list.Status.Equals("PAID", StringComparison.OrdinalIgnoreCase))
                    {
                        list.Status = "DONE";
                    }
                    else list.Status = "CANCELLED";

                     _context.Orders.Update(list);
                    
                }
                await _context.SaveChangesAsync();

                var mapped = listOrder.Select(l => _mapper.Map<ChangeStatusOrderToCompletedResponseModel>(l)).ToList();
                return mapped;
            }
            return null;
        }

        public async Task<int> TotalOrderInSystem()
        {

            int orderCount = await _context.Orders.CountAsync();
            return orderCount;
        }

        public async Task<List<GetAllOrderByMealSessionIdResponseModel>> GetAllOrderByMealSessionId(int mealsessionid)
        {
            var result = _context.Orders.Where(x => x.MealSessionId == mealsessionid).Select(x => new GetAllOrderByMealSessionIdResponseModel
            {
                OrderId=x.OrderId,
                MealSessionId=mealsessionid,
                CutomerDtoGetAllOrderByMealSessionId = new CutomerDtoGetAllOrderByMealSessionId
                {
                    CustomerId = x.Customer.CustomerId,
                    Name = x.Customer.Name,
                    AreaId = x.Customer.AreaId,
                    DistrictId = x.Customer.DistrictId,
                    Phone = x.Customer.Phone,
                    UserId = x.Customer.UserId,
                },
                Quantity=x.Quantity,
                Status=x.Status,
                Time=((DateTime)x.Time).ToString("dd-MM-yyyy HH:mm"),
                TotalPrice=x.TotalPrice,
            }).ToList();

            var mapped = result.Select(r => _mapper.Map<GetAllOrderByMealSessionIdResponseModel>(r)).ToList();
            
            return mapped;
        }
    }
}

