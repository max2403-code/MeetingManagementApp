using MeetingManagementApp.Storage.Entities;
using System.Text.Json;

namespace MeetingManagementApp.Storage.Context
{
    public class MeetingStorageContext
    {
        public IDictionary<int, Meeting> Meetings { get; } = new Dictionary<int, Meeting>();
        public IDictionary<int, MeetingNotification> MeetingNotifications { get; } = new Dictionary<int, MeetingNotification>();

        private const string _meetingStoragePath = "../Storage/meeting-storage.txt";
        private const string _meetingNotificationsStoragePath = "../Storage/meeting-notifications-storage.txt";



        public MeetingStorageContext() 
        {
            Meetings = GetMeetingStorageFromFile();
            MeetingNotifications = GetMeetingNotificationsStorageFromFile();
        }

        public async Task SaveMeetingStorageInfoAsync()
        {
            var file = new FileInfo(_meetingStoragePath);

            file.Directory.Create(); // If the directory already exists, this method does nothing.

            var content = JsonSerializer.Serialize(Meetings);

            await File.WriteAllTextAsync(file.FullName, content);

            await Task.Delay(5000);

        }

        public async Task SaveMeetingNotificationsStorageInfoAsync()
        {
            var file = new FileInfo(_meetingNotificationsStoragePath);

            file.Directory.Create(); // If the directory already exists, this method does nothing.

            var content = JsonSerializer.Serialize(MeetingNotifications);

            await File.WriteAllTextAsync(file.FullName, content);

            await Task.Delay(5000);

        }

        private IDictionary<int, Meeting> GetMeetingStorageFromFile()
        {
            if (!File.Exists(_meetingStoragePath))
                return new Dictionary<int, Meeting>();

            string meetingsText;

            using (var reader = new StreamReader(_meetingStoragePath))
            {
                meetingsText = reader.ReadToEnd();
            }

            var rval = JsonSerializer.Deserialize<IDictionary<int, Meeting>>(meetingsText);

            return rval ?? new Dictionary<int, Meeting>();
        }

        private IDictionary<int, MeetingNotification> GetMeetingNotificationsStorageFromFile()
        {
            if (!File.Exists(_meetingNotificationsStoragePath))
                return new Dictionary<int, MeetingNotification>();

            string meetingNotificationsText;

            using (var reader = new StreamReader(_meetingNotificationsStoragePath))
            {
                meetingNotificationsText = reader.ReadToEnd();
            }

            var rval = JsonSerializer.Deserialize<IDictionary<int, MeetingNotification>>(meetingNotificationsText);

            return rval ?? new Dictionary<int, MeetingNotification>();
        }
    }
}
