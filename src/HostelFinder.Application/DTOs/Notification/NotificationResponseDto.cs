
namespace HostelFinder.Application.DTOs.Notification
{
    public class NotificationResponseDto
    {
        public string Message { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string TimeAgo { get; set; }
    }
}
