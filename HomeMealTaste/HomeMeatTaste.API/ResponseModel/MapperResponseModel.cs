using AutoMapper;
using HomeMealTaste.Data.Models;


namespace HomeMealTaste.Services.ResponseModel
{
    public class MapperResponseModel : Profile
    {
        public MapperResponseModel()
        {
            CreateMap<UserResponseForgetPasswordModel, User>().ReverseMap();
            CreateMap<UserResponseModel, User>().ReverseMap();
            CreateMap<UserResponseUpdatePasswordAccountModel, User>().ReverseMap();
        }
    }
}
