using CorePush.Google;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using static HomeMealTaste.Data.ResponseModel.GoogleNotification;

namespace HomeMealTaste.Services.Implement

{
    public class NotificationSerivce : INotificationService
    {
        private readonly FcmNotificationSetting _fcmNotificationSetting;
        private readonly IConfiguration _configuration;
        private readonly HomeMealTasteContext _context;
        public NotificationSerivce(IOptions<FcmNotificationSetting> settings, IConfiguration configuration, HomeMealTasteContext context)
        {
            _fcmNotificationSetting = settings.Value;
            _configuration = configuration;
            _context = context;
        }


        public async Task<ResponseModels> SendNotificationForOrderReadyAndMealSessionCompleted(int mealSessionId)
        {
            ResponseModels response = new ResponseModels();
            try
            {

                /* FCM Sender (Android Device) */
                FcmSettings settings = new FcmSettings()
                {
                    SenderId = _configuration["FcmNotification:SenderId"],
                    ServerKey = _configuration["FcmNotification:ServerKey"],
                };
                HttpClient httpClient = new HttpClient();

                var mealSession = _context.MealSessions.Where(x => x.MealSessionId == mealSessionId).FirstOrDefault();
                var mealName = _context.Meals.Where(x => x.MealId == mealSession.MealId).Select(x => x.Name).FirstOrDefault();
                var listCustomerId = _context.Orders.Where(x => x.MealSessionId == mealSessionId).Select(x => x.CustomerId).ToList();
                var userId = _context.Customers.Where(x => listCustomerId.Contains(x.CustomerId)).Select(x => x.UserId).ToList();
                var getDeviceToken = _context.Users.Where(x => userId.Contains(x.UserId)).Select(x => x.DeviceToken).ToList();
                var getOrder = _context.Orders.Where(x => x.MealSessionId == mealSessionId).ToList();
                //if (Status.Equals("READY"))
                //{
                //    mealSession.Status = "COMPLETED";
                //    _context.MealSessions.Update(mealSession);
                //    await _context.SaveChangesAsync();

                //    foreach (var order in getOrder)
                //    {
                //        order.Status = "READY";
                //        _context.Orders.Update(order);
                //        await _context.SaveChangesAsync();
                //    }
                //}

                foreach (var tokens in getDeviceToken)
                {
                    string authorizationKey = string.Format("key={0}", settings.ServerKey);
                    string deviceToken = tokens;

                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationKey);
                    httpClient.DefaultRequestHeaders.Accept
                            .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    DataPayload dataPayload = new DataPayload();
                    dataPayload.Title = $"Order Ready";
                    dataPayload.Body = $"Your Order is Ready, please come to eat";

                    GoogleNotification notification = new GoogleNotification();
                    notification.Data = dataPayload;
                    notification.Notification = dataPayload;

                    var fcm = new FcmSender(settings, httpClient);
                    var fcmSendResponse = await fcm.SendAsync(deviceToken, notification);

                    if (fcmSendResponse.IsSuccess())
                    {
                        response.IsSuccess = true;
                        response.Message = "Notification sent successfully";
                        return response;
                    }
                    else
                    {
                        // Log the detailed response for debugging
                        Console.WriteLine("FCM Send Response: " + JsonConvert.SerializeObject(fcmSendResponse));

                        response.IsSuccess = false;
                        response.Message = fcmSendResponse.Results[0].Error;
                        return response;
                    }

                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Something went wrong";
                return response;
            }
            return null;
        }

        public async Task<ResponseModels> SendNotificationForOrderAcceptedAndMealSessionMaking(int mealSessionId)
        {
            ResponseModels response = new ResponseModels();
            try
            {

                /* FCM Sender (Android Device) */
                FcmSettings settings = new FcmSettings()
                {
                    SenderId = _configuration["FcmNotification:SenderId"],
                    ServerKey = _configuration["FcmNotification:ServerKey"],
                };
                HttpClient httpClient = new HttpClient();

                var mealSession = _context.MealSessions.Where(x => x.MealSessionId == mealSessionId).FirstOrDefault();
                var mealName = _context.Meals.Where(x => x.MealId == mealSession.MealId).Select(x => x.Name).FirstOrDefault();
                var listCustomerId = _context.Orders.Where(x => x.MealSessionId == mealSessionId).Select(x => x.CustomerId).ToList();
                var userId = _context.Customers.Where(x => listCustomerId.Contains(x.CustomerId)).Select(x => x.UserId).ToList();
                var getDeviceToken = _context.Users.Where(x => userId.Contains(x.UserId)).Select(x => x.DeviceToken).ToList();
                var getOrder = _context.Orders.Where(x => x.MealSessionId == mealSessionId).ToList();
                //if (Status.Equals("READY"))
                //{
                //    mealSession.Status = "COMPLETED";
                //    _context.MealSessions.Update(mealSession);
                //    await _context.SaveChangesAsync();

                //    foreach (var order in getOrder)
                //    {
                //        order.Status = "READY";
                //        _context.Orders.Update(order);
                //        await _context.SaveChangesAsync();
                //    }
                //}

                foreach (var tokens in getDeviceToken)
                {
                    string authorizationKey = string.Format("key={0}", settings.ServerKey);
                    string deviceToken = tokens;

                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationKey);
                    httpClient.DefaultRequestHeaders.Accept
                            .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    DataPayload dataPayload = new DataPayload();
                    dataPayload.Title = $"Order Accepted";
                    dataPayload.Body = $"Your Order is Accepted by Kitchen";

                    GoogleNotification notification = new GoogleNotification();
                    notification.Data = dataPayload;
                    notification.Notification = dataPayload;

                    var fcm = new FcmSender(settings, httpClient);
                    var fcmSendResponse = await fcm.SendAsync(deviceToken, notification);

                    if (fcmSendResponse.IsSuccess())
                    {
                        response.IsSuccess = true;
                        response.Message = "Notification sent successfully";
                        return response;
                    }
                    else
                    {
                        // Log the detailed response for debugging
                        Console.WriteLine("FCM Send Response: " + JsonConvert.SerializeObject(fcmSendResponse));

                        response.IsSuccess = false;
                        response.Message = fcmSendResponse.Results[0].Error;
                        return response;
                    }

                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Something went wrong";
                return response;
            }
            return null;
        }
    }
}

