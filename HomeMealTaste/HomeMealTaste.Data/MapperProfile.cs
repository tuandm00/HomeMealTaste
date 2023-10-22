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
<<<<<<< HEAD
            CreateMap<UserRegisterCustomerRequestModel, User>().ReverseMap();
            CreateMap<UserRegisterChefRequestModel, User>().ReverseMap();
=======
>>>>>>> b5d3d86b2421729cebc7bbf139da2dcfe1b40eff
            CreateMap<SessionRequestModel, Session>().ReverseMap();
            CreateMap<MealSessionRequestModel, MealSession>().ReverseMap();


            CreateMap<UserResponseForgetPasswordModel, User>().ReverseMap();
            CreateMap<UserResponseModel, User>().ReverseMap();
            CreateMap<UserResponseUpdatePasswordAccountModel, User>().ReverseMap();
<<<<<<< HEAD
            CreateMap<UserRegisterCustomerResponseModel, User>().ReverseMap();
            CreateMap<UserRegisterChefResponseModel, User>().ReverseMap();
=======
>>>>>>> b5d3d86b2421729cebc7bbf139da2dcfe1b40eff
            CreateMap<SessionResponseModel, Session>().ReverseMap();
            CreateMap<MealSessionResponseModel, MealSession>().ReverseMap();
        }
    }
}
