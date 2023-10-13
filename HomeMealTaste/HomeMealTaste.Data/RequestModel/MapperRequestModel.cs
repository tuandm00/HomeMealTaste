using AutoMapper;
using HomeMealTaste.Data.Models;

namespace HomeMealTaste.Data.RequestModel
{
    public class MapperRequestModel : Profile
    {
        public MapperRequestModel()
        {
            CreateMap<UserRequestModel, User>().ReverseMap();
            CreateMap<UserRequestForgetPasswordModel, User>().ReverseMap();
            CreateMap<UserRequestUpdatePasswordAccountModel, User>().ReverseMap();
        }
    }
}
