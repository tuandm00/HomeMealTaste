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
            var orderIdByMealsessionId = _context.Orders.Where(x => x.MealSessionId == request.MealSessionId).Select(x => x.OrderId).ToList();
            var checkStatusOrder = _context.Orders.Select(x => x.Status).FirstOrDefault();
            var result = _context.Posts.Where(x => orderIdByMealsessionId.Contains((int)x.OrderId)).ToList();
            var customerId = _context.Orders.Where(x => orderIdByMealsessionId.Contains((int)x.OrderId)).Select(x => x.CustomerId).ToList();
            var userId = _context.Customers.Where(x => customerId.Contains(x.CustomerId)).Select(x => x.UserId).ToList();
            var userEmail = _context.Users.Where(x => userId.Contains(x.UserId)).Select(x => x.Email).ToList();

            if (userEmail.Any())
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (checkStatusOrder.Equals("DONE"))
                        {
                            foreach (var email in userEmail)
                            {
                                SendNotificationEmail(email, "Order Post", "A new Order for you is available. Check it out!");
                            }

                            foreach (var orderId in orderIdByMealsessionId)
                            {
                                var add = new Post
                                {
                                    OrderId = orderId,
                                    Status = "PUSHED",
                                };
                                _context.Posts.Add(add);
                            }
                        }
                        else throw new Exception("Can not Send Notification Because Status is CANCELLED or PAID");
                        

                        _context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        // Handle exception, log, or rethrow as needed
                    }
                }
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
