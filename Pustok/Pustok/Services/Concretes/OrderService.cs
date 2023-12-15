using Microsoft.AspNetCore.Http;
using Pustok.Contracts;
using Pustok.Database;
using Pustok.Database.DomainModels;
using Pustok.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Pustok.Services.Concretes
{
    public class OrderService : IOrderService
    {
        private readonly PustokDbContext pustokDbContext;
        private readonly IUserService userService;
        private readonly IHubContext httpContext;

        public OrderService(PustokDbContext pustokDbContext, IUserService userService)
        {
            this.pustokDbContext = pustokDbContext;
            this.userService = userService;
        }

        public string GenerateTrackingCode()
        {
            var random = new Random();
            string code;
            string numberPart;


            do
            {
                numberPart = random.Next(1, 100000).ToString();
                code = $"OR{numberPart.PadLeft(5, '0')}";
            } while (DoesCodeExist(code));

            return code;
        }

        public bool DoesCodeExist(string code)
        {
            return false;
        }

        public List<Notifications> CreatOrderNotifications(Order order)
        {
            var staff = userService.GetWholeStaff();
            List<Notifications> notifications = new List<Notifications>();

            foreach(var user in staff)
            {
                var notification = new Notifications
                {
                    Title = NotificationTemplate.Order.Created.TITLE,
                    Content = NotificationTemplate.Order.Created.CONTENT.
                              Replace(NotificationTemplateKeyword.TRACKING_CODE, order.TrackingCode).
                              Replace(NotificationTemplateKeyword.USER_FULL_NAME, userService.GetFullName(order.User)),
                    User = user,
                    CreatedDate = DateTime.UtcNow,
                };

                notifications.Add(notification);

                pustokDbContext.Notifications.Add(notification);
            }

            return notifications;

        }

        public List<UserNotifications> UpdateOrderStatusNotifications(Order order, OrderStatus status)
        {
            List<UserNotifications> notifications = new List<UserNotifications>();

                var notification = new UserNotifications
                {
                    Title = NotificationTemplate.OrderStatusUpdated.TITLE.
                            Replace(NotificationTemplateKeyword.USER_FULL_NAME, userService.GetFullName(order.User)).
                            Replace(NotificationTemplateKeyword.TRACKING_CODE, order.TrackingCode),
                    Content = NotificationTemplate.OrderStatusUpdated.CONTENT.
                              Replace(NotificationTemplateKeyword.ORDER_STATUS, status.ToString()),
                    User = order.User,
                    CreatedDate = DateTime.UtcNow,
                };

                notifications.Add(notification);

                pustokDbContext.UserNotifications.Add(notification);
   

            return notifications;

        }

    }
}
