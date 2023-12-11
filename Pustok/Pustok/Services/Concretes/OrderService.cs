using Pustok.Contracts;
using Pustok.Database;
using Pustok.Database.DomainModels;
using Pustok.Services.Abstract;
using System;
using System.Collections.Generic;

namespace Pustok.Services.Concretes
{
    public class OrderService : IOrderService
    {
        private readonly PustokDbContext pustokDbContext;
        private readonly IUserService userService;

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


    }
}
