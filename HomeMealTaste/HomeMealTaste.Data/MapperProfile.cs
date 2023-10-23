﻿using AutoMapper;
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


            CreateMap<UserResponseForgetPasswordModel, User>().ReverseMap();
            CreateMap<UserResponseModel, User>().ReverseMap();
            CreateMap<UserResponseUpdatePasswordAccountModel, User>().ReverseMap();
            CreateMap<UserRegisterCustomerResponseModel, User>().ReverseMap();
            CreateMap<UserRegisterChefResponseModel, User>().ReverseMap();
            CreateMap<SessionResponseModel, Session>().ReverseMap();
            CreateMap<MealSessionResponseModel, MealSession>().ReverseMap();
            CreateMap<DishResponseModel, Dish>().ReverseMap();
        }
    }
}
