﻿using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Services.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeMealTaste.Services.Helper;

namespace HomeMealTaste.Services.Interface
{
    public interface IDishService
    {
        Task<DishResponseModel> CreateDish(DishRequestModel dish);
        Task<PagedList<Dish>> GetAllDish(PagingParams pagingParams);
        Task<Dish> DeleteDishId(int id);
    }
}
