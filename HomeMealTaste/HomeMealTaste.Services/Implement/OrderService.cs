using AutoMapper;
using HomeMealTaste.Data.Helper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Interface;
using Microsoft.AspNetCore.Http;
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
                    Time = x.Time.ToString(),
                    Quantity = x.Quantity,
                    CustomerDto1 = new CustomerDto1
                    {
                        CustomerId = x.Customer.CustomerId,
                        Name = x.Customer.Name,
                        Phone = x.Customer.Phone,
                        DistrictId = x.Customer.DistrictId,
                        AreaId = x.Customer.AreaId,
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
                            CreateDate = x.MealSession.Session.CreateDate.ToString(),
                            StartTime = x.MealSession.Session.StartTime.ToString(),
                            EndTime = x.MealSession.Session.EndTime.ToString(),
                            EndDate = x.MealSession.Session.EndDate.ToString(),
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
                    Time = x.Time.ToString(),
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
                            CreateDate = x.MealSession.Session.CreateDate.ToString(),
                            StartTime = x.MealSession.Session.StartTime.ToString(),
                            EndTime = x.MealSession.Session.EndTime.ToString(),
                            EndDate = x.MealSession.Session.EndDate.ToString(),
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
                }).FirstOrDefault();

            return Task.FromResult(results);
        }

        public async Task<List<GetAllOrderByUserIdResponseModel>> GetAllOrderByCustomerId(int id)
        {
            var results = _context.Orders.Include(x => x.MealSession).Where(x => x.CustomerId == id).Select(x => new GetAllOrderByUserIdResponseModel
            {
                OrderId = x.OrderId,
                Time = x.Time.ToString(),

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
                        CreateDate = GetDateTimeTimeZoneVietNam().ToString("dd-MM-yyyy"),
                        StartTime = GetDateTimeTimeZoneVietNam().ToString("HH:mm"),
                        EndTime = GetDateTimeTimeZoneVietNam().ToString("HH:mm"),
                        EndDate = GetDateTimeTimeZoneVietNam().ToString("dd-MM-yyyy"),
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
                    Time = x.Time.ToString(),
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
                        MealId = x.MealSession.MealId,
                        SessionDto = new SessionDto
                        {
                            SessionId = x.MealSession.Session.SessionId,
                            CreateDate = x.MealSession.Session.CreateDate,
                            StartTime = x.MealSession.Session.StartTime,
                            EndTime = x.MealSession.Session.EndTime,
                            EndDate = x.MealSession.Session.EndDate,
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
                        CreateDate = x.MealSession.CreateDate,
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
            var customerid = _context.Customers.Where(customer => customer.CustomerId == entity.CustomerId).FirstOrDefault();
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
            walletid.Balance = (int?)(walletid.Balance - totalprice);

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

            //save to table transaction

            var transactionid = _context.Orders
                .Select(x => new Transaction
                {
                    OrderId = x.OrderId,
                    WalletId = walletid.WalletId,
                    Date = createOrder.Time,
                    Amount = (decimal?)totalprice,
                    Description = "DONE WITH PAYMENT",
                    Status = "SUCCEED",
                }).AsNoTracking().FirstOrDefault();

            _context.Transactions.Add(transactionid);

            var orderEntity = _mapper.Map<Order>(createOrder);
            await _context.AddAsync(orderEntity);
            await _context.SaveChangesAsync();
            transaction.Commit();
            var mapped = _mapper.Map<CreateOrderResponseModel>(orderEntity);
            return mapped;
        }


    }
}
