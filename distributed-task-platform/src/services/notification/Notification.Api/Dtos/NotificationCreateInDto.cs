namespace Notification.Api.Dtos
{
    public class NotificationCreateInDto
    {
        public int FromUserNumber { get; set; }

        public string MsgTitle { get; set; }

        public string MsgText { get; set; }

        
        public List<int> lstReciveUserNumber { get; set; }
    }
}
