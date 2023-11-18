using AutoMapper;
using HomeMealTaste.Data.Implement;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Services.Implement
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        private HomeMealTasteContext _context;

        public PostService(IPostRepository postRepository, IMapper mapper, HomeMealTasteContext context)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _context = context;
        }


        public async Task<PostResponseModel> CreatePostStatusAfterOrder(PostRequestModel createPostRequest)
        {
            var entity = _mapper.Map<Post>(createPostRequest);
            //var paidOrderID = await _context.Orders
            //    .Where(x => x.Status == "Paid")
            //    .Select(x => x.OrderId)
            //    .FirstOrDefaultAsync();
            //entity.OrderId = paidOrderID;
            //var nameOfMeal = await _context.Orders
            //    .Where(x=> x.OrderId==paidOrderID)
            //    .Select(x=>x.MealSession.Meal.Name)
            //    .FirstOrDefaultAsync();
            switch (createPostRequest.Status)
            {
                case "Processing":
                case "Ready":
                case "Done":
                    entity.Status = createPostRequest.Status; break;
                default:
                    entity.Status = "Processing";
                    break;
            }
            entity.OrderId = createPostRequest.OrderId;

            await _context.Posts.AddAsync(entity);
            await _context.SaveChangesAsync();
            var mapper = _mapper.Map<PostResponseModel>(entity);
            return mapper;
        }
    }
}
