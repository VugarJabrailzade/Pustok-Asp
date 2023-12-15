namespace Pustok.Contracts;

public class NotificationTemplate
{
    public class Order
    {
        public class Created
        {
            public const string TITLE = "New order created";
            public const string CONTENT = $"The is a new order, #{NotificationTemplateKeyword.TRACKING_CODE} from {NotificationTemplateKeyword.USER_FULL_NAME}";
        }
    }

    public class OrderStatusUpdated
    {
        public const string TITLE = $"{NotificationTemplateKeyword.USER_FULL_NAME} your #{NotificationTemplateKeyword.TRACKING_CODE}  order status updated";
        public const string CONTENT = $"A New order status is \"{NotificationTemplateKeyword.ORDER_STATUS}\"";
    }
}

public class NotificationTemplateKeyword
{
    public const string TRACKING_CODE = "{order_tracking_code}";
    public const string USER_FULL_NAME = "{user_full_name}";
    public const string ORDER_STATUS = "{order_status}";

}
