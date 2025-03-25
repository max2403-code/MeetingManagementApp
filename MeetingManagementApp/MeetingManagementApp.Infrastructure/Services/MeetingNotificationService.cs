using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Models.DTO;
using MeetingManagementApp.Storage.Context;
using MeetingManagementApp.Storage.Entities;
using System.Collections.Concurrent;

namespace MeetingManagementApp.Infrastructure.Services
{
    public class MeetingNotificationService : INotificationService
    {
        private readonly MeetingStorageContext _context;
        private readonly ConcurrentDictionary<int, MeetingNotificationDTO> _notifications;
        private readonly int _delay = 10;

        public MeetingNotificationService(MeetingStorageContext context)
        {
            _context = context;
            _notifications = new ConcurrentDictionary<int, MeetingNotificationDTO>(_context.MeetingNotifications.Select(x => new KeyValuePair<int, MeetingNotificationDTO>(x.Key, new MeetingNotificationDTO
            {
                MeetingId = x.Value.MeetingId,
                NotificationTime = x.Value.NotificationTime
            })).ToArray());
        }

        public int AddNewMeetingNotification(MeetingNotificationDTO notification)
        {
            var entity = new MeetingNotification
            {
                MeetingId = notification.MeetingId,
                NotificationTime = notification.NotificationTime
            };

            _context.MeetingNotifications[notification.MeetingId] = entity;

            _notifications.AddOrUpdate(notification.MeetingId, notification, (k, v) => notification);

            return 1;
        }

        public int UpdateMeetingNotification(MeetingNotificationDTO notification)
        {
            _context.MeetingNotifications[notification.MeetingId].NotificationTime = notification.NotificationTime;

            _notifications.AddOrUpdate(notification.MeetingId, notification, (k, v) => notification);

            return 1;
        }

        public string? ValidateMeetingNotification(MeetingNotificationDTO notification)
        {
            if (!_context.Meetings.TryGetValue(notification.MeetingId, out var meeting))
                return "Встречи для данного уведомления не существует.";

            if (notification.NotificationTime < DateTime.Now || notification.NotificationTime >= meeting.MeetingStart)
                return "Указано неверное время уведомления.";

            return null;
        }

        public bool RemoveMeetingNotification(int meetingId)
        {
            if (!_context.MeetingNotifications.Remove(meetingId))
                return false;

            _notifications.Remove(meetingId, out MeetingNotificationDTO? value);

            return true;
        }

        public IReadOnlyCollection<MeetingNotificationDTO> GetMeetingNotifications()
        {
            var notificationRangeFrom = DateTime.Now;
            var notificationRangeTo = notificationRangeFrom.AddSeconds(_delay);

            return _notifications.Where(x => x.Value.NotificationTime >= notificationRangeFrom && x.Value.NotificationTime <= notificationRangeTo).Select(x => x.Value).ToArray();
        }
    }
}
