﻿using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Services.Interface
{
    public interface IPostService
    {
        Task<PostResponseModel> CreatePostStatusAfterOrder(PostRequestModel createPostRequest);
        Task<string> getMealNameByOrderId(int OrderId);
    }
}
