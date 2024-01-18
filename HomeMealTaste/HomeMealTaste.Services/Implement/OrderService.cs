using AutoMapper;
using HomeMealTaste.Data.Helper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Interface;

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
        private readonly IPostService _postService;
        private readonly HomeMealTasteContext _context;
        public OrderService(IOrderRepository orderRepository, IMapper mapper, HomeMealTasteContext context, IPostService postService)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _context = context;
            _postService = postService;
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
                .Include(x => x.MealSession).ThenInclude(x => x.Session).ThenInclude(x => x.SessionAreas).ThenInclude(x => x.Area)
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
                            CreateDate = ((DateTime)x.MealSession.Meal.CreateDate).ToString("dd-MM-yyyy"),
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
                            AreaId = x.MealSession.Session.SessionAreas.FirstOrDefault().Area.AreaId,
                        },
                        Price = x.MealSession.Price,
                        Quantity = x.MealSession.Quantity,
                        RemainQuantity = x.MealSession.RemainQuantity,
                        Status = x.MealSession.Status,
                        CreateDate = ((DateTime)x.MealSession.CreateDate).ToString("dd-MM-yyyy"),
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
                    Time = ((DateTime)x.Time).ToString("dd-MM-yyyy HH:mm"),
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
                            CreateDate = ((DateTime)x.MealSession.Meal.CreateDate).ToString("dd-MM-yyyy"),
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
                            AreaId = x.MealSession.Session.SessionAreas.FirstOrDefault().Area.AreaId,
                        },
                        Price = x.MealSession.Price,
                        Quantity = x.MealSession.Quantity,
                        RemainQuantity = x.MealSession.RemainQuantity,
                        Status = x.MealSession.Status,
                        CreateDate = ((DateTime)x.MealSession.CreateDate).ToString("dd-MM-yyyy"),
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
                Time = ((DateTime)x.Time).ToString("dd-MM-yyyy HH:mm"),

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
                        CreateDate = ((DateTime)x.MealSession.Meal.CreateDate).ToString("dd-MM-yyyy"),
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
                        AreaId = x.MealSession.Session.SessionAreas.FirstOrDefault().Area.AreaId,
                    },
                    Price = x.MealSession.Price,
                    Quantity = x.MealSession.Quantity,
                    RemainQuantity = x.MealSession.RemainQuantity,
                    Status = x.MealSession.Status,
                    CreateDate = ((DateTime)x.MealSession.CreateDate).ToString("dd-MM-yyyy"),
                },
                Status = x.Status,
                TotalPrice = x.TotalPrice,
                Quantity = x.Quantity,

            }).ToList();

            var mappedResults = results.Select(order => _mapper.Map<GetAllOrderByUserIdResponseModel>(order)).ToList();
            return mappedResults;
        }

        public Task<List<GetOrderByKitchenIdResponseModel>> GetAllOrderByKitchenId(int kitchenid)
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
                                AreaId = x.MealSession.Session.SessionAreas.FirstOrDefault().Area.AreaId,
                                Address = x.MealSession.Session.SessionAreas.FirstOrDefault().Area.Address,
                                AreaName = x.MealSession.Session.SessionAreas.FirstOrDefault().Area.AreaName,
                                DistrictDtoOrderResponse = new DistrictDtoOrderResponse
                                {
                                    DistrictId = x.MealSession.Session.SessionAreas.FirstOrDefault().Area.District.DistrictId,
                                    DistrictName = x.MealSession.Session.SessionAreas.FirstOrDefault().Area.District.DistrictName,
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
                    Quantity = x.Quantity,
                });

            var mapped = result.Select(x => _mapper.Map<GetOrderByKitchenIdResponseModel>(x)).ToList();
            return Task.FromResult(mapped);


        }

        //public async Task<CreateOrderResponseModel> CreateOrder(CreateOrderRequestModel createOrderRequest)
        //{
        //    using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? transaction = _context.Database.BeginTransaction();
        //    var entity = _mapper.Map<Order>(createOrderRequest);
        //    var mealSessionIdInOrder = _context.Orders.Select(x => x.MealSessionId).ToList();
        //    //foreach (var id in mealSessionIdInOrder)
        //    //{
        //    //    if (entity.MealSessionId != id)
        //    //    {
        //    //        var sessionCheck1 = _context.MealSessions.Where(x => x.MealSessionId == entity.MealSessionId).Select(x => x.SessionId).FirstOrDefault();
        //    //        var sessionCheck2 = _context.MealSessions.Where(x => x.MealSessionId == id).Select(x => x.SessionId).FirstOrDefault();
        //    //        //var sessionCheck1 = _context.MealSessions
        //    //        //    .Where(x => x.MealSessionId == entity.MealSessionId)
        //    //        //    .Select(x => new { x.SessionId, x.Status })
        //    //        //    .FirstOrDefault();

        //    //        //var sessionCheck2 = _context.MealSessions
        //    //        //    .Where(x => x.MealSessionId == id)
        //    //        //    .Select(x => new { x.SessionId, x.Status })
        //    //        //    .FirstOrDefault();
        //    //        //if (sessionCheck1 != null && sessionCheck2 != null &&
        //    //        //    sessionCheck1.SessionId == sessionCheck2.SessionId &&
        //    //        //    sessionCheck1.Status == sessionCheck2.Status)
        //    //        //{
        //    //        //    throw new Exception("Cannot Order in the Same Session with the Same Status");
        //    //        //}
        //    //        if (sessionCheck1 == sessionCheck2)
        //    //        {
        //    //            throw new Exception("Can Not Order In Same Session");
        //    //        }
        //    //    }
        //    //}
        //    var customerid = _context.Customers.Where(customerid => customerid.UserId == entity.CustomerId).FirstOrDefault();
        //    var mealsessionid = _context.MealSessions
        //        .Where(mealsession => mealsession.MealSessionId == entity.MealSessionId && mealsession.Status.Equals("APPROVED"))
        //        .Include(mealsession => mealsession.Meal)
        //            .ThenInclude(meal => meal.MealDishes)
        //            .ThenInclude(mealDish => mealDish.Dish)
        //        .AsNoTracking().FirstOrDefault();

        //    var walletid = _context.Wallets
        //        .Include(user => user.User)
        //        .ThenInclude(customer => customer.Customers)
        //        .Where(x => x.UserId == x.User.UserId).FirstOrDefault();

        //    if (mealsessionid == null)
        //    {
        //        throw new Exception("Session is not start");
        //    }
        //    if (mealsessionid.RemainQuantity == 0)
        //    {
        //        throw new Exception("No meal can order because the quantity is over");
        //    }
        //    var price = mealsessionid.Price;
        //    var remainquantity = mealsessionid.RemainQuantity;
        //    mealsessionid.RemainQuantity = remainquantity - createOrderRequest.Quantity;
        //    var totalprice = price * createOrderRequest.Quantity;
        //    //check mealsessionid then add order to table order
        //    var createOrder = new CreateOrderRequestModel
        //    {

        //        CustomerId = entity.CustomerId,
        //        TotalPrice = (int?)totalprice,
        //        Time = GetDateTimeTimeZoneVietNam(),
        //        Status = "PAID",
        //        MealSessionId = mealsessionid.MealSessionId,
        //        Quantity = createOrderRequest.Quantity,
        //    };

        //    var customer = _context.Customers.Where(z => z.CustomerId == createOrder.CustomerId).FirstOrDefault();
        //    var user = _context.Users.Where(x => x.UserId == customer.UserId).FirstOrDefault();
        //    var walletCustomer = _context.Wallets.Where(x => x.UserId == user.UserId).FirstOrDefault();
        //    var walletOfUserIdOfCustomer = _context.Wallets.Where(x => x.UserId == user.UserId).Select(x => x.WalletId).FirstOrDefault();
        //    if (walletCustomer != null)
        //    {
        //        var afterBalanceCustomer = (int?)(walletCustomer.Balance - totalprice);

        //        if (afterBalanceCustomer >= 0)
        //        {
        //            walletCustomer.Balance = afterBalanceCustomer;
        //            _context.Wallets.Update(walletCustomer);
        //        }
        //        else
        //        {
        //            throw new Exception("YOUR BALANCE IS OUT OF AMOUNT");
        //        }
        //    }
        //    // save to admin wallet take 10%
        //    var admin = _context.Users.Where(x => x.RoleId == 1 && x.UserId == 2).FirstOrDefault();
        //    var priceToAdmin = (totalprice * 10) / 100;

        //    if (admin != null)
        //    {
        //        // Check if the admin already has a wallet
        //        var adminWallet = _context.Wallets.FirstOrDefault(w => w.UserId == admin.UserId);

        //        if (adminWallet != null)
        //        {
        //            // Update the existing wallet
        //            adminWallet.Balance += (int?)priceToAdmin;
        //            _context.Wallets.Update(adminWallet);
        //        }

        //    }

        //    //then transfer price after 10 % of admin to kitchen
        //    var kitchen = _context.MealSessions
        //        .Where(x => x.MealSessionId == entity.MealSessionId)
        //        .Include(x => x.Kitchen)
        //        .AsNoTracking()
        //        .FirstOrDefault();
        //    var kitchenid = _context.MealSessions.Where(x => x.MealSessionId == entity.MealSessionId).Select(x => x.KitchenId).FirstOrDefault();
        //    var userIdChef = _context.Kitchens.Where(x => x.KitchenId == kitchenid).Select(x => x.UserId).FirstOrDefault();
        //    var walletOfUserIdOfKitchen = _context.Wallets.Where(x => x.UserId == userIdChef).Select(X => X.WalletId).FirstOrDefault();
        //    var priceToChef = totalprice - priceToAdmin;

        //    if (kitchen != null)
        //    {
        //        var chefWallet = _context.Wallets.FirstOrDefault(w => w.UserId == kitchen.Kitchen.UserId);

        //        if (chefWallet != null)
        //        {
        //            // Update the existing wallet
        //            chefWallet.Balance += (int?)priceToChef;
        //            _context.Wallets.Update(chefWallet);
        //        }

        //    }

        //    _context.MealSessions.Update(mealsessionid);
        //    _context.Wallets.Update(walletid);

        //    var orderEntity = _mapper.Map<Order>(createOrder);
        //    await _context.AddAsync(orderEntity);
        //    await _context.SaveChangesAsync();

        //    // Save to transaction for admin
        //    var transactionToAdmin = new Transaction
        //    {
        //        OrderId = orderEntity.OrderId,
        //        WalletId = walletid.WalletId,
        //        Date = createOrder.Time,
        //        Amount = (decimal?)priceToAdmin,
        //        Description = "DONE WITH REVENUE",
        //        Status = "SUCCEED",
        //        TransactionType = "RR",
        //        UserId = admin.UserId,
        //    };

        //    _context.Transactions.Add(transactionToAdmin);

        //    // Save to transaction for chef
        //    var transactionToChef = new Transaction
        //    {
        //        OrderId = orderEntity.OrderId,
        //        WalletId = walletOfUserIdOfKitchen,
        //        Date = createOrder.Time,
        //        Amount = (decimal?)priceToChef,
        //        Description = "DONE WITH REVENUE",
        //        Status = "SUCCEED",
        //        TransactionType = "RR",
        //        UserId = kitchen.Kitchen.UserId,
        //    };

        //    _context.Transactions.Add(transactionToChef);


        //    var useridwallet = _context.Wallets.Select(x => x.UserId).FirstOrDefault();
        //    //save to table transaction
        //    var transactionid = new Transaction
        //    {
        //        OrderId = orderEntity.OrderId,
        //        WalletId = walletOfUserIdOfCustomer,
        //        Date = createOrder.Time,
        //        Amount = (decimal?)totalprice,
        //        Description = "DONE WITH PAYMENT",
        //        Status = "SUCCEED",
        //        TransactionType = "ORDERED",
        //        UserId = user.UserId,
        //    };

        //    _context.Transactions.Add(transactionid);

        //    await _context.SaveChangesAsync();
        //    transaction.Commit();
        //    var mapped = _mapper.Map<CreateOrderResponseModel>(orderEntity);
        //    return mapped;
        //}

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


        public async Task<CreateOrderResponseModel> CreateOrder(CreateOrderRequestModel createOrderRequest)
        {
            using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? transaction = _context.Database.BeginTransaction();
            var entity = _mapper.Map<Order>(createOrderRequest);

            var customerid = _context.Customers.Where(customerid => customerid.CustomerId == entity.CustomerId).FirstOrDefault();
            var mealsessionid = _context.MealSessions
                .Where(mealsession => mealsession.MealSessionId == entity.MealSessionId && mealsession.Status.Equals("APPROVED") && (mealsession.Session.Status.Equals("BOOKING")))
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
                var mealsessions = _context.MealSessions.Where(x => x.MealSessionId == entity.MealSessionId).FirstOrDefault();
                var sessions = _context.Sessions.Where(x => x.SessionId == mealsessions.SessionId).FirstOrDefault();
                throw new Exception($"Can not Create Order because status of MealSession is {mealsessions.Status} and status of Session is {sessions.Status}");
            }
            if (mealsessionid.RemainQuantity == 0)
            {
                throw new Exception("No meal can order because the quantity is over");
            }
            var price = mealsessionid.Price;
            var remainquantity = mealsessionid.RemainQuantity;
            mealsessionid.RemainQuantity = remainquantity - createOrderRequest.Quantity;
            var totalprice = price * createOrderRequest.Quantity;

            _context.MealSessions.Update(mealsessionid);

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
            var walletOfUserIdOfCustomer = _context.Wallets.Where(x => x.UserId == user.UserId).Select(x => x.WalletId).FirstOrDefault();
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

            // cong tien(totalprice) vo vi admin
            var userIdOfAdmin = _context.Users.Where(x => x.UserId == 2).Select(x => x.UserId).FirstOrDefault();
            var walletOfUserIdOfAdmin = _context.Wallets.Where(x => x.UserId == userIdOfAdmin).Select(x => x.WalletId).FirstOrDefault();
            var walletAdmin = _context.Wallets.Where(x => x.UserId == userIdOfAdmin).FirstOrDefault();
            int afterBalanceAdmin = 0;
            if (walletAdmin != null)
            {
                afterBalanceAdmin = (int)totalprice;
                if (afterBalanceAdmin >= 0)
                {
                    walletAdmin.Balance += afterBalanceAdmin;
                    _context.Wallets.Update(walletAdmin);
                }
            }
            _context.Wallets.Update(walletid);

            

            var orderEntity = _mapper.Map<Order>(createOrder);
            await _context.AddAsync(orderEntity);
            await _context.SaveChangesAsync();


            var useridwallet = _context.Wallets.Select(x => x.UserId).FirstOrDefault();
            //save to table transaction
            var transactionid = new Transaction
            {
                OrderId = orderEntity.OrderId,
                WalletId = walletOfUserIdOfCustomer,
                Date = createOrder.Time,
                Amount = (decimal?)totalprice,
                Description = "ORDER SUCCESS",
                Status = "SUCCEED",
                TransactionType = "ORDERED",
                UserId = user.UserId,
            };

            var transactionAdmin = new Transaction
            {
                OrderId = orderEntity.OrderId,
                WalletId = walletOfUserIdOfAdmin,
                Date = createOrder.Time,
                Amount = (decimal?)totalprice,
                Description = $"RECEIVE MONEY WHEN CUSTOMER {customer.Name} CREATE ORDER",
                Status = "SUCCEED",
                TransactionType = "TRANSFER",
                UserId = user.UserId,
            };
            _context.Transactions.Add(transactionid);

            await _context.SaveChangesAsync();
            transaction.Commit();
            var mapped = _mapper.Map<CreateOrderResponseModel>(orderEntity);
            return mapped;
        }

        public async Task<List<ChangeStatusOrderToCompletedResponseModel>> ChangeStatusOrder(int mealsessionid, string status)
        {
            var listOrder = await _context.Orders.Where(x => x.MealSessionId == mealsessionid).ToListAsync();
            var mealSession = await _context.MealSessions.Where(x => x.MealSessionId == mealsessionid).FirstOrDefaultAsync();
            int? count = 0;
            foreach (var order in listOrder)
            {
                count += order.Quantity;
            }
            if (listOrder != null)
            {
                foreach (var order in listOrder)
                {
                    if (status.Equals("ACCEPTED", StringComparison.OrdinalIgnoreCase) && order.Status.Equals("PAID", StringComparison.OrdinalIgnoreCase))
                    {
                        order.Status = "ACCEPTED";
                        mealSession.Status = "MAKING";
                    }
                    else if (status.Equals("READY", StringComparison.OrdinalIgnoreCase) && order.Status.Equals("ACCEPTED", StringComparison.OrdinalIgnoreCase))
                    {
                        order.Status = "READY";
                        mealSession.Status = "COMPLETED";
                        //cam thong bao mealsession vao day
                    }

                    else if (status.Equals("CANCELLED", StringComparison.OrdinalIgnoreCase) && order.Status.Equals("PAID", StringComparison.OrdinalIgnoreCase))
                    {
                        order.Status = "CANCELLED";
                        mealSession.Status = "CANCELLED";

                        if (count >= (mealSession.Quantity * 0.4))
                        {
                            // ham hoan tien customer, tru tien chef
                            await RefundMoneyToSingleCustomerByOrderIdWhenChefCancelledOrderWithBookingSlotEnough(order.OrderId);
                        }
                        else
                        {
                            // hoan tien customer , ko tru tien chef
                            await RefundMoneyToSingleCustomerByOrderIdWhenChefCancelledOrderWithBookingSlotNotEnough(order.OrderId);
                        }
                    };

                    _context.MealSessions.Update(mealSession);
                    _context.Orders.Update(order);
                    await _context.SaveChangesAsync();

                }
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
                OrderId = x.OrderId,
                MealSessionId = mealsessionid,
                CutomerDtoGetAllOrderByMealSessionId = new CutomerDtoGetAllOrderByMealSessionId
                {
                    CustomerId = x.Customer.CustomerId,
                    Name = x.Customer.Name,
                    AreaId = x.Customer.AreaId,
                    DistrictId = x.Customer.DistrictId,
                    Phone = x.Customer.Phone,
                    UserId = x.Customer.UserId,
                },
                Quantity = x.Quantity,
                Status = x.Status,
                Time = ((DateTime)x.Time).ToString("dd-MM-yyyy HH:mm"),
                TotalPrice = x.TotalPrice,
            }).ToList();

            var mapped = result.Select(r => _mapper.Map<GetAllOrderByMealSessionIdResponseModel>(r)).ToList();

            return mapped;
        }

        public async Task<int> GetTotalPriceWithMealSessionByMealSessionId(int mealsessionid)
        {
            var getMealsessionId = await _context.Orders.Where(x => x.MealSessionId == mealsessionid).ToListAsync();
            int? sum = 0;
            if (getMealsessionId != null)
            {
                foreach (var q in getMealsessionId)
                {
                    if (q.Status.Equals("PAID", StringComparison.OrdinalIgnoreCase) || q.Status.Equals("DONE", StringComparison.OrdinalIgnoreCase))
                    {
                        sum += q.TotalPrice;
                    }
                }
                return (int)sum;
            }
            return 0;
        }

        public async Task<int> GetTotalPriceWithMealSessionBySessionIdAndKitchenId(int sessionId, int kitchenId)
        {
            var datenow = GetDateTimeTimeZoneVietNam();
            var listMealSession = _context.MealSessions.Where(x => x.SessionId == sessionId && x.KitchenId == kitchenId && x.Status.Equals("APPROVED") && x.CreateDate.Value.Date == datenow.Date).ToList();
            int sum = 0;
            if (listMealSession != null)
            {
                foreach (var i in listMealSession)
                {
                    int totalPrice = await GetTotalPriceWithMealSessionByMealSessionId(i.MealSessionId);

                    if (totalPrice != 0)
                    {
                        sum += totalPrice;
                    }
                }
                return sum;
            }
            else throw new Exception("Can not Found");
        }

        public async Task RefundMoneyToSingleCustomerByOrderIdWhenChefCancelledOrderWithBookingSlotEnough(int orderId)
        {
            using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? transaction = _context.Database.BeginTransaction();
            var orderitem = _context.Orders.Where(x => x.OrderId == orderId).FirstOrDefault();
            var datenow = GetDateTimeTimeZoneVietNam();
            //update wallet admin
            var userIdsOfAdmin = _context.Users.Where(x => x.RoleId == 1 && x.UserId == 2).Select(x => x.UserId).FirstOrDefault();
            var getCustomer = _context.Orders.Where(x => x.OrderId == orderId).FirstOrDefault();
            var getUserIdByCustomer = _context.Customers.Where(x => x.CustomerId == getCustomer.CustomerId).FirstOrDefault();


            var mealSessionId = _context.Orders.Where(x => x.OrderId == orderId).Select(x => x.MealSessionId).FirstOrDefault();
            var getKitchen = _context.MealSessions.Where(x => x.MealSessionId == mealSessionId).FirstOrDefault();
            var walletIdsOfAdmin = _context.Wallets.Where(x => x.UserId == userIdsOfAdmin).FirstOrDefault();
            if (walletIdsOfAdmin != null)
            {
                walletIdsOfAdmin.Balance = (int?)(walletIdsOfAdmin.Balance - (orderitem.TotalPrice) + (orderitem.TotalPrice * 0.1));
                _context.Wallets.Update(walletIdsOfAdmin);
            }
            var addToTransactionForAdmin = new Transaction
            {
                OrderId = orderId,
                WalletId = walletIdsOfAdmin.WalletId,
                Date = datenow,
                Amount = ((decimal?)(orderitem.TotalPrice * 0.1)),
                Description = $"REFUND TO CUSTOMER {getUserIdByCustomer.Name}",
                Status = "SUCCEED",
                TransactionType = "REFUND",
                UserId = userIdsOfAdmin,
            };
            _context.Transactions.Add(addToTransactionForAdmin);

            //update wallet customer
            var getWalletOfCustomer = _context.Wallets.Where(x => x.UserId == getUserIdByCustomer.UserId).FirstOrDefault();
            getWalletOfCustomer.Balance += orderitem.TotalPrice;
            _context.Wallets.Update(getWalletOfCustomer);

            var addToTransactionForCustomer = new Transaction
            {
                OrderId = orderId,
                WalletId = getWalletOfCustomer.WalletId,
                Date = datenow,
                Amount = orderitem.TotalPrice,
                Description = "REFUND",
                Status = "SUCCEED",
                TransactionType = "REFUND",
                UserId = getUserIdByCustomer.UserId,
            };
            _context.Transactions.Add(addToTransactionForCustomer);
            //update wallet chef Tru tien
           
            var getUserIdByKitchenId = _context.Kitchens.Where(x => x.KitchenId == getKitchen.KitchenId).Select(x => x.UserId).FirstOrDefault();
            var getWalletOfKitchen = _context.Wallets.Where(x => x.UserId == getUserIdByKitchenId).FirstOrDefault();
            getWalletOfKitchen.Balance = (int?)(getWalletOfKitchen.Balance - (orderitem.TotalPrice * 0.1));
            _context.Wallets.Update(getWalletOfCustomer);

            var addToTransactionForChef = new Transaction
            {
                OrderId = orderId,
                WalletId = getWalletOfKitchen.WalletId,
                Date = datenow,
                Amount = ((decimal?)(orderitem.TotalPrice * 0.1)),
                Description = "FINED",
                Status = "SUCCEED",
                TransactionType = "FINED",
                UserId = getUserIdByKitchenId,
            };
            _context.Transactions.Add(addToTransactionForChef);

            //tao transaction hoan tien luu ve vi cua customer, admin :refund , vi cua chef fined 

            await _context.SaveChangesAsync();
            transaction.Commit();
        }
        public async Task RefundMoneyToSingleCustomerByOrderIdWhenChefCancelledOrderWithBookingSlotNotEnough(int orderId)
        {
            using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? transaction = _context.Database.BeginTransaction();
            var orderitem = _context.Orders.Where(x => x.OrderId == orderId).FirstOrDefault();
            var datenow = GetDateTimeTimeZoneVietNam();
            //update wallet admin
            var userIdsOfAdmin = _context.Users.Where(x => x.RoleId == 1 && x.UserId == 2).Select(x => x.UserId).FirstOrDefault();
            var getCustomer = _context.Orders.Where(x => x.OrderId == orderId).FirstOrDefault();
            var getUserIdByCustomer = _context.Customers.Where(x => x.CustomerId == getCustomer.CustomerId).FirstOrDefault();


            
            var walletIdsOfAdmin = _context.Wallets.Where(x => x.UserId == userIdsOfAdmin).FirstOrDefault();
            if (walletIdsOfAdmin != null)
            {
                walletIdsOfAdmin.Balance = (int?)(walletIdsOfAdmin.Balance - (orderitem.TotalPrice));
                _context.Wallets.Update(walletIdsOfAdmin);
            }

            var addToTransactionForAdmin = new Transaction
            {
                OrderId = orderId,
                WalletId = walletIdsOfAdmin.WalletId,
                Date = datenow,
                Amount = (orderitem.TotalPrice),
                Description = $"REFUND TO CUSTOMER {getUserIdByCustomer.Name}",
                Status = "SUCCEED",
                TransactionType = "REFUND",
                UserId = userIdsOfAdmin,
            };
            _context.Transactions.Add(addToTransactionForAdmin);

            //update wallet customer
            
            var getWalletOfCustomer = _context.Wallets.Where(x => x.UserId == getUserIdByCustomer.UserId).FirstOrDefault();
            getWalletOfCustomer.Balance += orderitem.TotalPrice;
            _context.Wallets.Update(getWalletOfCustomer);

            var addToTransactionForCustomer = new Transaction
            {
                OrderId = orderId,
                WalletId = getWalletOfCustomer.WalletId,
                Date = datenow,
                Amount = (orderitem.TotalPrice),
                Description = "REFUND",
                Status = "SUCCEED",
                TransactionType = "REFUND",
                UserId = getUserIdByCustomer.UserId,
            };
            _context.Transactions.Add(addToTransactionForCustomer);
            //tao transaction hoan tien luu ve vi cua customer, admin :refund , vi cua chef fined 

            await _context.SaveChangesAsync();
            transaction.Commit();
        }
        public async Task ChefCancelledOrderRefundMoneyToCustomerV2(int mealsessionId)
        {
            using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? transaction = _context.Database.BeginTransaction();
            var getListOrder = await GetAllOrderByMealSessionId(mealsessionId);
            var sessionid = _context.MealSessions.Where(x => x.MealSessionId == mealsessionId).Select(x => x.SessionId).FirstOrDefault();
            var sessionStatus = _context.Sessions.Where(x => x.SessionId == sessionid).Select(x => x.Status).FirstOrDefault();
            if (sessionStatus == null || sessionStatus.Equals(""))
            {
                throw new Exception("Can not CANCEL");
            }
            foreach (var item in getListOrder)
            {
                var orderitem = _context.Orders.Where(x => x.OrderId == item.OrderId).FirstOrDefault();
                orderitem.Status = "CANCELLED";
                _context.Orders.Update(orderitem);
            }


            var userIdsOfAdmin = _context.Users.Where(x => x.RoleId == 1 && x.UserId == 2).Select(x => x.UserId).FirstOrDefault();
            var walletIdsOfAdmin = _context.Wallets.Where(x => x.UserId == userIdsOfAdmin).FirstOrDefault();
            var countTotalPrice = _context.Orders.Where(x => x.MealSessionId == mealsessionId).Select(x => x.TotalPrice).ToList().Sum();

            if (walletIdsOfAdmin != null)
            {
                walletIdsOfAdmin.Balance = (int?)(walletIdsOfAdmin.Balance - (countTotalPrice) + (countTotalPrice * 0.1));
                _context.Wallets.Update(walletIdsOfAdmin);
            }


            var getListCustomerId = _context.Orders.Where(x => x.MealSessionId == mealsessionId).Select(x => x.CustomerId).ToList();
            var getListUserIdByCustomerId = _context.Customers.Where(x => getListCustomerId.Contains(x.CustomerId)).Select(x => x.UserId).ToList();
            var getListWalletOfCustomer = _context.Wallets.Where(x => getListUserIdByCustomerId.Contains(x.UserId)).ToList();
            if (getListWalletOfCustomer != null)
            {
                foreach (var cus in getListWalletOfCustomer)
                {
                    var listTotalPriceOfCustomer = _context.Orders.Where(x => getListCustomerId.Contains(x.CustomerId) && x.MealSessionId == mealsessionId).Select(x => x.TotalPrice).FirstOrDefault();
                    cus.Balance = cus.Balance + listTotalPriceOfCustomer;
                    _context.Wallets.Update(cus);
                }
            }

            var getListKitchenId = _context.MealSessions.Where(x => x.MealSessionId == mealsessionId).Select(x => x.KitchenId).ToList();
            var getListUserIdByKitchenId = _context.Kitchens.Where(x => getListKitchenId.Contains(x.KitchenId)).Select(x => x.UserId).ToList();
            var getListWalletOfKitchen = _context.Wallets.Where(x => getListUserIdByKitchenId.Contains(x.UserId)).ToList();

            if (getListWalletOfKitchen != null)
            {
                foreach (var chefWallet in getListWalletOfKitchen)
                {
                    chefWallet.Balance = (int?)(chefWallet.Balance - (countTotalPrice * 0.1));
                    _context.Wallets.Update(chefWallet);
                }
            }






            await _context.SaveChangesAsync();
            transaction.Commit();
        }

        public async Task<int> TotalPriceOfOrderInSystemInEveryMonth(int month)
        {
            decimal? totalPriceInMonth = await _context.Orders
        .Where(x => x.Time.HasValue && x.Time.Value.Month == month)
        .SumAsync(x => x.TotalPrice);

            return (int)totalPriceInMonth;
        }

        public async Task<List<TotalPriceOfOrderInSystemWithEveryMonthResponseModel>> TotalPriceOfOrderInSystemWithEveryMonth()
        {
            var totalPriceByMonth = await _context.Orders
        .Where(x => x.Time.HasValue)
        .GroupBy(x => x.Time.Value.Month)
        .Select(group => new TotalPriceOfOrderInSystemWithEveryMonthResponseModel
        {
            Month = group.Key,
            TotalPrice = group.Sum(x => x.TotalPrice)
        })
        .ToListAsync();

            // Generate a range of months from 1 to 12
            var allMonths = Enumerable.Range(1, 12);

            // Left join the grouped data with the range of months
            var result = allMonths
                .GroupJoin(totalPriceByMonth,
                    m => m,
                    t => t?.Month ?? 0, // Handle null Month values
                    (month, totals) => new TotalPriceOfOrderInSystemWithEveryMonthResponseModel
                    {
                        Month = month,
                        TotalPrice = totals.Sum(t => t?.TotalPrice) ?? 0
                    })
                .ToList();

            return result;
        }

        public async Task<int> TotalCustomerOrderInSystem()
        {
            var totalCustomerOrder = await _context.Orders.Select(x => x.CustomerId).CountAsync();

            return totalCustomerOrder;
        }

        public async Task<List<GetTop5CustomerOrderTimesResponseModel>> GetTop5CustomerOrderTimes()
        {
            var top5OrderTimes = await _context.Orders.Include(x => x.Customer)
                .GroupBy(x => x.CustomerId)
                .Select(group => new
                {
                    CustomerDtoGetTop5 = new CustomerDtoGetTop5
                    {
                        UserId = group.Key,
                        Name = group.First().Customer.Name,
                        AreaId = group.First().Customer.AreaId,
                        CustomerId = group.First().Customer.CustomerId,
                        DistrictId = group.First().Customer.DistrictId,
                        Phone = group.First().Customer.Phone,
                    },
                    CustomerId = group.Key,
                    OrderTimes = group.Count()
                })
                .OrderByDescending(x => x.OrderTimes)
                .Take(5)
                .Select(x => new GetTop5CustomerOrderTimesResponseModel
                {
                    CustomerDtoGetTop5 = x.CustomerDtoGetTop5,
                    OrderTimes = x.OrderTimes
                })
                .ToListAsync();

            return top5OrderTimes;
        }

        public async Task<List<GetTop5ChefOrderTimesResponseModel>> GetTop5ChefOrderTimes()
        {
            var listMealSessionIds = _context.MealSessions
                .Select(x => x.MealSessionId)
                .ToList();

            if (listMealSessionIds != null && listMealSessionIds.Any())
            {
                var top5OrderTimes = await _context.Orders
                    .Where(order => listMealSessionIds.Contains((int)order.MealSessionId))
                    .GroupBy(order => order.MealSession.KitchenId)
                    .Select(group => new GetTop5ChefOrderTimesResponseModel
                    {
                        ChefDtoGetTop5 = new ChefDtoGetTop5
                        {
                            KitchenId = (int)group.Key,
                            Name = group.First().MealSession.Kitchen.Name,
                            UserId = group.First().MealSession.Kitchen.UserId,
                            Address = group.First().MealSession.Kitchen.Address,
                            AreaId = group.First().MealSession.Kitchen.AreaId,
                        },
                        OrderTimes = group.Count()
                    })
                    .OrderByDescending(x => x.OrderTimes)
                    .Take(5)
                    .ToListAsync();

                return top5OrderTimes;
            }

            return new List<GetTop5ChefOrderTimesResponseModel>();
        }

        public async Task<List<GetAllOrderByUserIdResponseModel>> GetAllOrderByUserId(int userId)
        {
            var customerId = _context.Customers.Where(x => x.UserId == userId).Select(x => x.CustomerId).FirstOrDefault();
            var results = _context.Orders.Include(x => x.MealSession).Where(x => x.CustomerId == customerId).Select(x => new GetAllOrderByUserIdResponseModel
            {
                OrderId = x.OrderId,
                Time = ((DateTime)x.Time).ToString("dd-MM-yyyy HH:mm"),

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
                        CreateDate = ((DateTime)x.MealSession.Meal.CreateDate).ToString("dd-MM-yyyy"),
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
                        AreaId = x.MealSession.Session.SessionAreas.FirstOrDefault().Area.AreaId,
                    },
                    Price = x.MealSession.Price,
                    Quantity = x.MealSession.Quantity,
                    RemainQuantity = x.MealSession.RemainQuantity,
                    Status = x.MealSession.Status,
                    CreateDate = ((DateTime)x.MealSession.CreateDate).ToString("dd-MM-yyyy"),
                },
                Status = x.Status,
                TotalPrice = x.TotalPrice,
                Quantity = x.Quantity,

            }).ToList();

            var mappedResults = results.Select(order => _mapper.Map<GetAllOrderByUserIdResponseModel>(order)).ToList();
            return mappedResults;
        }

        public async Task<List<GetAllOrderWithStatusPaidByMealSessionIdResponseModel>> GetAllOrderWithStatusPaidByMealSessionId(int mealsessionId)
        {
            var result = _context.Orders.Where(x => x.MealSessionId == mealsessionId && x.Status.Equals("PAID")).Select(x => new GetAllOrderWithStatusPaidByMealSessionIdResponseModel
            {
                MealSessionId = x.MealSessionId,
                CustomerId = x.CustomerId,
                OrderId = x.OrderId,
                Quantity = x.Quantity,
                Status = x.Status,
                Time = ((DateTime)x.Time).ToString("dd-MM-yyyy HH:mm"),
                TotalPrice = x.TotalPrice,
            }).ToList();

            var mapped = result.Select(r => _mapper.Map<GetAllOrderWithStatusPaidByMealSessionIdResponseModel>(r)).ToList();
            return mapped;
        }

        public async Task<List<OrderResponseModel>> GetAllOrderWithStatusCompleted()
        {
            var result = _context.Orders
                .Include(x => x.MealSession.Meal.Kitchen)
                .Include(x => x.MealSession).ThenInclude(x => x.Session).ThenInclude(x => x.SessionAreas).ThenInclude(x => x.Area)
                .Where(x => x.Status.Equals("COMPLETED"))
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
                            CreateDate = ((DateTime)x.MealSession.Meal.CreateDate).ToString("dd-MM-yyyy"),
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
                            AreaId = x.MealSession.Session.SessionAreas.FirstOrDefault().Area.AreaId,
                        },
                        Price = x.MealSession.Price,
                        Quantity = x.MealSession.Quantity,
                        RemainQuantity = x.MealSession.RemainQuantity,
                        Status = x.MealSession.Status,
                        CreateDate = ((DateTime)x.MealSession.CreateDate).ToString("dd-MM-yyyy"),
                    },
                    Status = x.Status,
                    Price = x.TotalPrice,
                });
            var mappedResult = result.Select(x => _mapper.Map<OrderResponseModel>(x)).ToList();

            return mappedResult;
        }

        public async Task<ChangeStatusOrderResponseModel> ChangeSingleStatusOrder(int orderId, string status)
        {
            var result = _context.Orders.Where(x => x.OrderId == orderId).FirstOrDefault();

            if (result == null)
            {
                throw new Exception("Can not find Order");
            }

            var mealsessionId = _context.Orders.Where(x => x.OrderId == orderId).Select(x => x.MealSessionId).FirstOrDefault();
            var sessionId = _context.MealSessions.Where(x => x.MealSessionId == mealsessionId).Select(x => x.SessionId).FirstOrDefault();
            var statusSession = _context.Sessions.Where(x => x.SessionId == sessionId).Select(x => x.Status).FirstOrDefault();

            if (statusSession != null && statusSession.Equals("ONGOING"))
            {
                if (result != null && status.Equals("COMPLETED", StringComparison.OrdinalIgnoreCase) && result.Status.Equals("READY"))
                {
                    result.Status = status.ToUpper();
                }

                else if (result != null && status.Equals("NOTEAT", StringComparison.OrdinalIgnoreCase) && result.Status.Equals("READY"))
                {
                    result.Status = status.ToUpper();
                }
            }
            else
            {
                throw new Exception("Status Session is Not ONGOING");
            }

            await _context.SaveChangesAsync();

            var mapped = _mapper.Map<ChangeStatusOrderResponseModel>(result);
            return mapped;
        }

        public async Task<List<GetAllOrderBySessionIdResponseModel>> GetAllOrderBySessionId(int sessionId)
        {
            var getMealSessionId = _context.MealSessions.Where(x => x.SessionId == sessionId).Select(x => x.MealSessionId).FirstOrDefault();
            var result = _context.Orders
                .Include(x => x.MealSession)
                .Where(x => x.MealSessionId == getMealSessionId).Select(x => new GetAllOrderBySessionIdResponseModel
                {
                    OrderId = x.OrderId,
                    Quantity = x.Quantity,
                    Status = x.Status,
                    Time = ((DateTime)x.Time).ToString("dd-MM-yyyy HH:mm"),
                    TotalPrice = x.TotalPrice,
                    CustomerDtoForGetAllOrderBySessionId = new CustomerDtoForGetAllOrderBySessionId
                    {
                        CustomerId = x.Customer.CustomerId,
                        Name = x.Customer.Name,
                        DistrictId = x.Customer.DistrictId,
                        Phone = x.Customer.Phone,
                        UserId = x.Customer.UserId,
                        AreaDtoForGetAllOrderBySessionId = new AreaDtoForGetAllOrderBySessionId
                        {
                            AreaId = x.Customer.User.Area.AreaId,
                            Address = x.Customer.User.Area.Address,
                            AreaName = x.Customer.User.Area.AreaName,
                            DistrictId = x.Customer.User.Area.DistrictId,
                        },
                    },
                    MealSessionDtoGetAllOrderBySessionId = new MealSessionDtoGetAllOrderBySessionId
                    {
                        MealSessionId = x.MealSession.MealSessionId,
                        CreateDate = ((DateTime)x.MealSession.CreateDate).ToString("dd-MM-yyyy"),
                        Price = x.MealSession.Price,
                        Quantity = x.MealSession.Quantity,
                        RemainQuantity = x.MealSession.RemainQuantity,
                        Status = x.MealSession.Status,
                        MealDtoForGetAllOrderBySessionId = new MealDtoForGetAllOrderBySessionId
                        {
                            MealId = x.MealSession.Meal.MealId,
                            CreateDate = ((DateTime)x.MealSession.Meal.CreateDate).ToString("dd-MM-yyyy"),
                            Description = x.MealSession.Meal.Description,
                            Image = x.MealSession.Meal.Image,
                            Name = x.MealSession.Meal.Name,
                            KitchenDtoForGetAllOrderBySessionIdenId = new KitchenDtoForGetAllOrderBySessionId
                            {
                                KitchenId = x.MealSession.Meal.Kitchen.KitchenId,
                                Name = x.MealSession.Meal.Kitchen.Name,
                                Address = x.MealSession.Meal.Kitchen.Address,
                                AreaId = x.MealSession.Meal.Kitchen.AreaId,
                                DistrictId = x.MealSession.Meal.Kitchen.DistrictId,
                                UserId = x.MealSession.Meal.Kitchen.UserId,
                            }
                        },
                        AreaDtoForGetAllOrderBySessionId = new AreaDtoForGetAllOrderBySessionId
                        {
                            AreaId = x.MealSession.Area.AreaId,
                            Address = x.MealSession.Area.Address,
                            AreaName = x.MealSession.Area.AreaName,
                            DistrictId = x.MealSession.Area.DistrictId,
                        },
                        KitchenDtoForGetAllOrderBySessionId = new KitchenDtoForGetAllOrderBySessionId
                        {
                            KitchenId = x.MealSession.Kitchen.KitchenId,
                            Name = x.MealSession.Kitchen.Name,
                            Address = x.MealSession.Kitchen.Address,
                            AreaId = x.MealSession.Kitchen.AreaId,
                            DistrictId = x.MealSession.Kitchen.DistrictId,
                            UserId = x.MealSession.Kitchen.UserId,
                        },
                        SessionDtoGetAllOrderBySessionId = new SessionDtoGetAllOrderBySessionId
                        {
                            SessionId = x.MealSession.Session.SessionId,
                            CreateDate = ((DateTime)x.MealSession.Session.CreateDate).ToString("dd-MM-yyyy"),
                            StartTime = ((DateTime)x.MealSession.Session.StartTime).ToString("HH: mm"),
                            EndTime = ((DateTime)x.MealSession.Session.EndTime).ToString("HH: mm"),
                            EndDate = ((DateTime)x.MealSession.Session.EndDate).ToString("dd-MM-yyyy"),
                            SessionName = x.MealSession.Session.SessionName,
                            SessionType = x.MealSession.Session.SessionType,
                            UserId = x.MealSession.Session.UserId,
                            Status = x.MealSession.Session.Status,
                        }
                    }
                }).ToList();

            var mapped = result.Select(r => _mapper.Map<GetAllOrderBySessionIdResponseModel>(r)).ToList();
            return mapped;
        }

        public async Task ChefCancelledNotEnoughOrderRefundMoneyToCustomerV2(int mealsessionId)
        {
            using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? transaction = _context.Database.BeginTransaction();
            var getListOrder = await GetAllOrderByMealSessionId(mealsessionId);
            var sessionid = _context.MealSessions.Where(x => x.MealSessionId == mealsessionId).Select(x => x.SessionId).FirstOrDefault();
            var sessionStatus = _context.Sessions.Where(x => x.SessionId == sessionid).Select(x => x.Status).FirstOrDefault();
            if (sessionStatus == null || sessionStatus.Equals(""))
            {
                throw new Exception("Can not CANCEL");
            }
            foreach (var item in getListOrder)
            {
                var orderitem = _context.Orders.Where(x => x.OrderId == item.OrderId).FirstOrDefault();
                orderitem.Status = "CANCELLED";
                _context.Orders.Update(orderitem);
            }

            var userIdsOfAdmin = _context.Users.Where(x => x.RoleId == 1 && x.UserId == 2).Select(x => x.UserId).FirstOrDefault();
            var walletIdsOfAdmin = _context.Wallets.Where(x => x.UserId == userIdsOfAdmin).FirstOrDefault();
            var countTotalPrice = _context.Orders.Where(x => x.MealSessionId == mealsessionId).Select(x => x.TotalPrice).ToList().Sum();

            if (walletIdsOfAdmin != null)
            {
                walletIdsOfAdmin.Balance = (int?)(walletIdsOfAdmin.Balance - (countTotalPrice));
                _context.Wallets.Update(walletIdsOfAdmin);
            }


            var getListCustomerId = _context.Orders.Where(x => x.MealSessionId == mealsessionId).Select(x => x.CustomerId).ToList();
            var getListUserIdByCustomerId = _context.Customers.Where(x => getListCustomerId.Contains(x.CustomerId)).Select(x => x.UserId).ToList();
            var getListWalletOfCustomer = _context.Wallets.Where(x => getListUserIdByCustomerId.Contains(x.UserId)).ToList();
            if (getListWalletOfCustomer != null)
            {
                foreach (var cus in getListWalletOfCustomer)
                {
                    var listTotalPriceOfCustomer = _context.Orders.Where(x => getListCustomerId.Contains(x.CustomerId) && x.MealSessionId == mealsessionId).Select(x => x.TotalPrice).FirstOrDefault();
                    cus.Balance = cus.Balance + listTotalPriceOfCustomer;
                    _context.Wallets.Update(cus);
                }
            }

            await _context.SaveChangesAsync();
            transaction.Commit();
        }

        public async Task<List<ChangeStatusOrderResponseModel>> ChangeListStatusOrderToCancelledForAdmin(ChangeListStatusOrderToCancelledForAdminRequestModel request)
        {
            var datenow = GetDateTimeTimeZoneVietNam();
            var getListOrderIds = _context.Orders
                .Where(x => request.OrderIds.Contains(x.OrderId) && x.Time.Value.Date == datenow.Date)
                .ToList();

            var getListMealSessionId = _context.Orders
                .Where(x => request.OrderIds.Contains(x.OrderId))
                .Select(x => x.MealSessionId)
                .ToList();

            var getListMealSessionStatus = _context.MealSessions
                .Where(x => getListMealSessionId.Contains(x.MealSessionId))
                .ToList();

            var getSessionIds = _context.MealSessions
                .Where(x => getListMealSessionId.Contains(x.MealSessionId))
                .Select(x => x.SessionId)
                .Distinct()
                .ToList();

            var getListSessionStatus = _context.Sessions
                .Where(x => getSessionIds.Contains(x.SessionId))
                .ToList();

            var isAnySessionBooking = getListSessionStatus.Any(session => session.Status.Equals("BOOKING"));
            var isAnySessionOngoing = getListSessionStatus.Any(session => session.Status.Equals("ONGOING"));

            if (getListOrderIds.Count > 0)
            {
                if (isAnySessionBooking)
                {
                    foreach (var order in getListOrderIds)
                    {
                        if (order.Status.Equals("PAID") || order.Status.Equals("ACCEPTED"))
                        {
                            order.Status = "CANCELLED";
                            await RefundMoneyToSingleCustomerByOrderIdWhenChefCancelledOrderWithBookingSlotEnough(order.OrderId);
                        }
                        else
                        {
                            throw new Exception("Can not change to CANCELLED");
                        }
                    }

                    foreach (var mealSession in getListMealSessionStatus)
                    {
                        mealSession.Status = "CANCELLED";
                    }
                }
                else if (isAnySessionOngoing)
                {
                    foreach (var order in getListOrderIds)
                    {
                        if (order.Status.Equals("READY"))
                        {
                            order.Status = "CANCELLED";
                            await RefundMoneyToSingleCustomerByOrderIdWhenChefCancelledOrderWithBookingSlotEnough(order.OrderId);
                        }
                        else
                        {
                            throw new Exception("Can not change to CANCELLED");
                        }
                    }

                    foreach (var mealSession in getListMealSessionStatus)
                    {
                        mealSession.Status = "CANCELLED";
                    }
                }

            }
            else
            {
                throw new Exception("Can not find Order");
            }

            await _context.SaveChangesAsync();

            var mapped = getListOrderIds.Select(g => _mapper.Map<ChangeStatusOrderResponseModel>(g)).ToList();
            return mapped;
        }

        public async Task ChangeStatusOrderToCancelledWhenOrderIsPaidByCustomer(int orderId)
        {
            var datenow = GetDateTimeTimeZoneVietNam();
            var checkOrderIsPaid = _context.Orders.Where(x => x.OrderId == orderId && x.Time.Value.Date == datenow.Date).FirstOrDefault();

            if (checkOrderIsPaid.Status.Equals("PAID"))
            {
                checkOrderIsPaid.Status = "CANCELLED";
                await RefundMoneyToSingleCustomerByOrderIdWhenCustomerCancelledOrderWithStatusPaid(orderId);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RefundMoneyToSingleCustomerByOrderIdWhenCustomerCancelledOrderWithStatusPaid(int orderId)
        {
            using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? transaction = _context.Database.BeginTransaction();
            var orderitem = _context.Orders.Where(x => x.OrderId == orderId).FirstOrDefault();
            var datenow = GetDateTimeTimeZoneVietNam();

            //update wallet admin
            var userIdsOfAdmin = _context.Users.Where(x => x.RoleId == 1 && x.UserId == 2).Select(x => x.UserId).FirstOrDefault();
            var getCustomer = _context.Orders.Where(x => x.OrderId == orderId).FirstOrDefault();
            var getUserIdByCustomer = _context.Customers.Where(x => x.CustomerId == getCustomer.CustomerId).FirstOrDefault();
            var walletIdsOfAdmin = _context.Wallets.Where(x => x.UserId == userIdsOfAdmin).FirstOrDefault();
            if (walletIdsOfAdmin != null)
            {
                walletIdsOfAdmin.Balance = (int?)(walletIdsOfAdmin.Balance - ((orderitem.TotalPrice) * 0.9));
                _context.Wallets.Update(walletIdsOfAdmin);
            }

            var addToTransactionForAdmin = new Transaction
            {
                OrderId = orderId,
                WalletId = walletIdsOfAdmin.WalletId,
                Date = datenow,
                Amount = (orderitem.TotalPrice),
                Description = $"REFUND TO CUSTOMER {getUserIdByCustomer.Name}",
                Status = "SUCCEED",
                TransactionType = "REFUND",
                UserId = userIdsOfAdmin,
            };
            _context.Transactions.Add(addToTransactionForAdmin);

            //update wallet customer
            
            var getWalletOfCustomer = _context.Wallets.Where(x => x.UserId == getUserIdByCustomer.UserId).FirstOrDefault();
            getWalletOfCustomer.Balance += (int?)((orderitem.TotalPrice) * 0.9);
            _context.Wallets.Update(getWalletOfCustomer);

            var addToTransactionForCustomer = new Transaction
            {
                OrderId = orderId,
                WalletId = getWalletOfCustomer.WalletId,
                Date = datenow,
                Amount = (orderitem.TotalPrice),
                Description = "REFUND With 90% Total Price Ordered",
                Status = "SUCCEED",
                TransactionType = "REFUND",
                UserId = getUserIdByCustomer.UserId,
            };
            _context.Transactions.Add(addToTransactionForCustomer);
            //tao transaction hoan tien luu ve vi cua customer, admin :refund , vi cua chef fined 

            await _context.SaveChangesAsync();
            transaction.Commit();
        }
    }
}

