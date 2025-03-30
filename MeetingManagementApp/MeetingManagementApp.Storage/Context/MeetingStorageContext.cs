using MeetingManagementApp.Storage.Entities;
using System.Text.Json;

namespace MeetingManagementApp.Storage.Context
{
    /// <summary>
    /// Имитация работы с БД.
    /// </summary>
    public class MeetingStorageContext
    {
        /// <summary>
        /// Хранилище встреч.
        /// </summary>
        public IDictionary<int, Meeting> Meetings { get; } = new Dictionary<int, Meeting>();

        /// <summary>
        /// Хранилище напоминаний о встречах.
        /// </summary>
        public IDictionary<int, MeetingNotification> MeetingNotifications { get; } = new Dictionary<int, MeetingNotification>();

        private const string _meetingStoragePath = "../Storage/meeting-storage.txt";
        private const string _meetingNotificationsStoragePath = "../Storage/meeting-notifications-storage.txt";

        public MeetingStorageContext() 
        {
            Meetings = GetMeetingStorageFromFile();
            MeetingNotifications = GetMeetingNotificationsStorageFromFile();
        }

        /// <summary>
        /// Сохранение информации о встречах в файл.
        /// </summary>
        /// <returns></returns>
        public async Task SaveMeetingStorageInfoAsync()
        {
            var file = new FileInfo(_meetingStoragePath);

            file.Directory.Create();

            var content = JsonSerializer.Serialize(Meetings);

            await File.WriteAllTextAsync(file.FullName, content);
        }

        /// <summary>
        /// Сохранение информации о напоминаниях в файл.
        /// </summary>
        /// <returns></returns>
        public async Task SaveMeetingNotificationsStorageInfoAsync()
        {
            var file = new FileInfo(_meetingNotificationsStoragePath);

            file.Directory.Create();

            var content = JsonSerializer.Serialize(MeetingNotifications);

            await File.WriteAllTextAsync(file.FullName, content);
        }

        /// <summary>
        /// Получение информации о встречах из файла.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Получение информации о напоминаниях из файла.
        /// </summary>
        /// <returns></returns>
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
