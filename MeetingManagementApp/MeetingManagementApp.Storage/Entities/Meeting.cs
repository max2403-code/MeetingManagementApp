namespace MeetingManagementApp.Storage.Entities
{
    public class Meeting
    {
        public int Id { get; set; }
        public DateTime MeetingStart { get; set; }
        public DateTime MeetingEnd { get; set; }
        public string Description { get; set; }
    }
}
