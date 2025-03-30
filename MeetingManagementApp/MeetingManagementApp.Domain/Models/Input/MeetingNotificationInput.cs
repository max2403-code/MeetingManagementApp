namespace MeetingManagementApp.Domain.Models.Input
{
    public class MeetingNotificationInput : Input
    {
        public int? MeetingId { get; set; }

        public DateTime? NotificationTime { get; set; }

        public DateTime? OnDate { get; set; }
    }
}
