using HomeMealTaste.Data.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Services.Interface
{
    public interface INotificationService
    {
        Task<ResponseModels> SendNotification(NotificationModel notificationModel);

    }
}
