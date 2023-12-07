using AutoMapper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Services.Implement
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly HomeMealTasteContext _context;
        private readonly IMapper _mapper;

        public PostService(IPostRepository postRepository, HomeMealTasteContext context, IMapper mapper)
        {
            _postRepository = postRepository;
            _context = context;
            _mapper = mapper;
        }

        public Task PostForAllCustomerWithOrderId(PostRequestModel request)
        {
            var entity = _mapper.Map<Post>(request);
            var result = _context.Posts.Where(x => x.OrderId == entity.OrderId).ToList();
            var orderId = request.OrderId;
            var customerId = _context.Orders.Where(x => x.OrderId == orderId).Select(x => x.CustomerId).SingleOrDefault();
            var userId = _context.Customers.Where(x => x.CustomerId == customerId).Select(x => x.UserId).SingleOrDefault();
            var userEmail = _context.Users.Where(x => x.UserId == userId).Select(x => x.Email).SingleOrDefault();

            if (userEmail != null)
            {
                SendNotificationEmail(userEmail, "Order Post", "A new Order for you is available. Check it out!");
                var add = new Post
                {
                    OrderId = orderId,
                    Status = "PUSHED",
                };
                _context.Posts.Add(add);
                _context.SaveChanges();
            }

            return Task.FromResult(result);
        }
    private void SendNotificationEmail(string toEmail, string subject, string body)
    {
        using (var client = new SmtpClient("smtp.gmail.com"))
        {
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("dominhtuan23102000@gmail.com", "djmr tgxz wfao upwq");
            client.EnableSsl = true;

            var message = new MailMessage("dominhtuan23102000@gmail.com", toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            client.Send(message);
        }
    }
}
}
