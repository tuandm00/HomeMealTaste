using AutoMapper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.ResponseModel;

namespace HomeMealTaste.Data.RequestModel
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserRequestModel, User>().ReverseMap();
            CreateMap<UserRequestForgetPasswordModel, User>().ReverseMap();
            CreateMap<UserRequestUpdatePasswordAccountModel, User>().ReverseMap();
            CreateMap<UserRegisterCustomerRequestModel, User>().ReverseMap();
            CreateMap<UserRegisterChefRequestModel, User>().ReverseMap();
            CreateMap<SessionRequestModel, Session>().ReverseMap();
            CreateMap<MealSessionRequestModel, MealSession>().ReverseMap();
            CreateMap<DishTypeRequestModel, DishType>().ReverseMap();
            CreateMap<DistrictRequestModel, District>().ReverseMap();
            CreateMap<UserResponseForgetPasswordModel, User>().ReverseMap();
            CreateMap<UserResponseModel, User>().ReverseMap();
            CreateMap<UserResponseUpdatePasswordAccountModel, User>().ReverseMap();
            CreateMap<UserRegisterCustomerResponseModel, User>().ReverseMap();
            CreateMap<UserRegisterChefResponseModel, User>().ReverseMap();
            CreateMap<SessionResponseModel, Session>().ReverseMap();
            CreateMap<MealSessionResponseModel, MealSession>().ReverseMap();
            CreateMap<DishTypeResponseModel, DishType>().ReverseMap();
            CreateMap<DistrictResponseModel, District>().ReverseMap();
            CreateMap<MealRequestModel, Meal>().ReverseMap();
            CreateMap<MealResponseModel, Meal>().ReverseMap();
            CreateMap<DishRequestModel, Dish>().ReverseMap();
            CreateMap<DishResponseModel, Dish>().ReverseMap();
            CreateMap<OrderResponseModel, Order>().ReverseMap();
            CreateMap<GetAllOrderByUserIdResponseModel, Order>().ReverseMap();
            CreateMap<KitchenResponseModel, Kitchen>().ReverseMap();
            CreateMap<AreaResponseModel, Area>().ReverseMap();
            CreateMap<AreaRequestModel, Area>().ReverseMap();
            CreateMap<GetDishIdByMealIdResponseModel, MealDish>().ReverseMap();
            CreateMap<GetAllSessionByAreaIdResponseModel, Session>().ReverseMap();
            CreateMap<GetAllUserWithRoleCustomerAndChefResponseModel, User>().ReverseMap();
            CreateMap<UpdateAreaRequestModel, Area>().ReverseMap();
            CreateMap<UpdateAreaResponseModel, Area>().ReverseMap();
            CreateMap<CreateOrderResponseModel, Order>().ReverseMap();
            CreateMap<CreateOrderRequestModel, Order>().ReverseMap();
            CreateMap<GetAllMealResponseModelNew, Meal>().ReverseMap();
            CreateMap<FeedbackRequestModel, Feedback>().ReverseMap();
            CreateMap<FeedbackResponseModel, Feedback>().ReverseMap();
            CreateMap<GetUserByIdResponseModel, User>().ReverseMap();
            CreateMap<GetAllTransactionByUserIdResponseModel, Transaction>().ReverseMap();
            CreateMap<GetDishByKitchenIdResponseModel, Dish>().ReverseMap();
            CreateMap<RefundMoneyToWalletByOrderIdRequestModel, Transaction>().ReverseMap();
            CreateMap<RefundMoneyToWalletByOrderIdResponseModel, Transaction>().ReverseMap();
            CreateMap<TransactionByUserIdRequestModel, Transaction>().ReverseMap();
            CreateMap<ChangeStatusOrderToCompletedResponseModel, Order>().ReverseMap();
            CreateMap<UpdateMealIdNotExistInSessionByMealIdResponseModel, Meal>().ReverseMap();
            CreateMap<UpdateMealIdNotExistInSessionByMealIdRequestModel, Meal>().ReverseMap();
            CreateMap<UpdateDishResponseModel, Dish>().ReverseMap();
            CreateMap<UpdateDishRequestModel, Dish>().ReverseMap();
            CreateMap<GetAllOrderByMealSessionIdResponseModel, Order>().ReverseMap();
            CreateMap<PostRequestModel, Post>().ReverseMap();
            CreateMap<PostResponseModel, Post>().ReverseMap();
            CreateMap<GetAllKitchenBySessionIdResponseModel, Kitchen>().ReverseMap();
            CreateMap<GetSingleSessionBySessionIdResponseModel, Session>().ReverseMap();
            CreateMap<GetTotalPriceWithMealSessionByMealSessionIdResponseModel, Order>().ReverseMap();
            CreateMap<GetAllTransactionsResponseModel, Transaction>().ReverseMap();
            CreateMap<TotalPriceOfOrderInSystemWithEveryMonthResponseModel, Order>().ReverseMap();
            CreateMap<UpdateUserRequestModel, User>().ReverseMap();
            CreateMap<UpdateUserResponseModel, User>().ReverseMap();
            CreateMap<TotalPriceOfOrderInSystemWithEveryMonthResponseModel, Order>().ReverseMap();
            CreateMap<GetSingleMealSessionByIdResponseModel, MealSession>().ReverseMap();
            CreateMap<GetAllDishInMealSessionByKitchenIdResponseModel, Dish>().ReverseMap();
            CreateMap<GetAllMealInMealSessionByKitchenIdResponseModel, Meal>().ReverseMap();

            

            //CreateMap<GetAllMealInCurrentSessionResponseModel.DishModel, Dish>().ReverseMap();
            //CreateMap<GetAllMealInCurrentSessionResponseModel.ChefInfo, Kitchen>().ReverseMap();
            //CreateMap<GetAllMealInCurrentSessionResponseModel.ChefInfo, Kitchen>().ReverseMap();
        }
    }
}
