namespace MeetingManagementApp.Domain.Models.DTO
{
    public class MeetingDTO
    {
        public int? Id { get; set; }
        public DateTime MeetingStart { get; set; }
        public DateTime MeetingEnd { get; set; }
        public string Subject { get; set; }

        public string Description { get; set; }
    }
}
