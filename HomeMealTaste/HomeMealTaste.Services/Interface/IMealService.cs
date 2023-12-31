﻿using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeMealTaste.Data.Helper;
using HomeMealTaste.Services.Helper;
using Microsoft.AspNetCore.Http;

namespace HomeMealTaste.Services.Interface
{
    public interface IMealService
    {
        Task<MealResponseModel> CreateMeal(MealRequestModel mealRequest);
        //Task<PagedList<GetAllMealResponseModel>> GetAllMeal(PagingParams pagingParams);
        Task<List<GetAllMealByKitchenIdResponseModel>> GetAllMealByKitchenId(int id);

        Task<GetMealByMealIdResponseModel> GetMealByMealId(int mealid);
        Task<List<GetAllMealResponseModelNew>> GetAllMeal();
        Task DeleteMealNotExistInSessionByMealId(int mealid);
        Task<UpdateMealIdNotExistInSessionByMealIdResponseModel> UpdateMealNotExistInSession(UpdateMealIdNotExistInSessionByMealIdRequestModel request);
        Task<List<GetAllMealInMealSessionByKitchenIdResponseModel>> GetAllMealInMealSessionByKitchenId(int kitchenid);

    }
}
