using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Models.DTO;
using MeetingManagementApp.Storage.Context;
using MeetingManagementApp.Storage.Entities;
using System.Collections.Concurrent;

namespace MeetingManagementApp.Infrastructure.Services
{
    internal class MeetingNotificationService : INotificationService
    {
        private readonly MeetingStorageContext _context;
        private readonly ConcurrentDictionary<int, MeetingNotificationDTO> _notifications;

        /// <summary>
        /// Диапазон отлавливания напоминаний для отправки.
        /// </summary>
        private readonly int _timeRange = 10;

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

        public bool RemoveMeetingNotification(int meetingId)
        {
            var result = _context.MeetingNotifications.Remove(meetingId) && _notifications.Remove(meetingId, out MeetingNotificationDTO? value);

            return result;
        }

        public MeetingNotificationDTO GetMeetingNotificationByMeetingId(int meetingId)
        {
            if (!_context.MeetingNotifications.TryGetValue(meetingId, out var value) || value == null)
                throw new Exception("Запрашиваемое уведомление отсутствует.");

            return new MeetingNotificationDTO
            {
                MeetingId = meetingId,
                NotificationTime = value.NotificationTime
            } ;
        }

        public IReadOnlyCollection<MeetingNotificationDTO> GetMeetingNotificationsWithTimeRange()
        {
            var notificationRangeFrom = DateTime.Now;
            var notificationRangeTo = notificationRangeFrom.AddSeconds(_timeRange);

            return _notifications.Where(x => x.Value.NotificationTime >= notificationRangeFrom && x.Value.NotificationTime <= notificationRangeTo).Select(x => x.Value).ToArray();
        }

        public string? ValidateMeetingNotificationTime(DateTime notificationTime, int meetingId)
        {
            if (!_context.Meetings.TryGetValue(meetingId, out var meeting))
                return "Встречи для данного уведомления не существует.";

            if (notificationTime < DateTime.Now || notificationTime > meeting.MeetingStart)
                return "Указано неверное время уведомления.";

            return null;
        }

        public string? ValidateMeetingNotificationOnDate(DateTime onDate, int meetingId)
        {
            if (!_context.Meetings.TryGetValue(meetingId, out var meeting))
                return "Встречи для данного уведомления не существует.";

            if (onDate < DateTime.Today || onDate > meeting.MeetingStart.Date)
                return "Указана неверная дата уведомления.";

            return null;
        }
    }
}
