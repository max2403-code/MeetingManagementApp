using MeetingManagementApp.Storage.Entities;

namespace MeetingManagementApp.Storage.Context
{
    public class MeetingStorageContext
    {
        public IDictionary<int, Meeting> Meetings { get; private set; } = new Dictionary<int, Meeting>();
        public IDictionary<int, MeetingNotification> MeetingNotifications { get; private set; } = new Dictionary<int, MeetingNotification>();

        public MeetingStorageContext() 
        { 
            // Прочитать из файла
        }
    }
}
