namespace MeetingManagementApp.Storage.Entities
{
    public class MeetingNotification
    {
        /// <summary>
        /// ID встречи.
        /// </summary>
        public int MeetingId { get; set; }

        /// <summary>
        /// Время уведомления.
        /// </summary>
        public DateTime NotificationTime { get; set; }
    }
}
