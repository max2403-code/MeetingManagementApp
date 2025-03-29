namespace MeetingManagementApp.Domain.Models.Input
{
    public class MeetingInput : Input
    {
        public int? Id { get; set; }
        public DateTime? OnDate { get; set; }
        public DateTime? MeetingStart { get; set; }
        public DateTime? MeetingEnd { get; set; }
        public string? Subject { get; set; }
        public string? Description { get; set; }
        public MeetingNotificationInput? MeetingNotification { get; set; }
    }
}
