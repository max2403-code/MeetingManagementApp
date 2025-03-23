using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Models.DTO;
using MeetingManagementApp.Storage.Context;
using MeetingManagementApp.Storage.Entities;

namespace MeetingManagementApp.Infrastructure.Services
{
    internal class MeetingService : IMeetingService
    {
        private readonly MeetingStorageContext _context;

        public MeetingService(MeetingStorageContext context) 
        {
            _context = context;
        }

        public IReadOnlyCollection<MeetingDTO> GetMeetingsOnDate(DateTime date)
        {
            var dateFrom = date.Date;
            var dateTo = dateFrom.AddDays(1);

            return _context.Meetings.Values
                .Where(x => date >= dateFrom && date < dateTo)
                .Select(x => new MeetingDTO
                {
                    Id = x.Id,
                    MeetingStart = x.MeetingStart,
                    MeetingEnd = x.MeetingEnd,
                    Description = x.Description
                })
                .ToArray();
        }

        public int AddNewMeeting(MeetingDTO meeting) 
        { 
            var entityIndex = _context.Meetings.Keys.DefaultIfEmpty().Max() + 1;

            var entity = new Meeting
            {
                Id = entityIndex,
                MeetingStart = meeting.MeetingStart,
                MeetingEnd = meeting.MeetingEnd,
                Description = meeting.Description
            };

            _context.Meetings[entityIndex] = entity;

            return entityIndex;
        }

        public string? ValidateMeeting(MeetingDTO meeting)
        {
            if (meeting.MeetingStart < DateTime.Now)
                return "Встречу можно планировать только в будущем.";

            var isMeetingExist = _context.Meetings.Values.Any(x =>
                x.MeetingStart <= meeting.MeetingStart && x.MeetingEnd >= meeting.MeetingStart ||
                x.MeetingStart <= meeting.MeetingEnd && x.MeetingEnd >= meeting.MeetingEnd ||
                x.MeetingStart <= meeting.MeetingStart && x.MeetingEnd >= meeting.MeetingEnd);

            if (isMeetingExist)
                return "На данный промежуток времени уже запланирована другая встреча.";

            if (!string.IsNullOrEmpty(meeting.Description))
                return "Описание встречи должно быть заполнено.";

            return null;
        }

        public int RemoveMeeting(int id) 
        {
            if (!_context.Meetings.Remove(id))
                throw new Exception("Данная встреча отсутствует в списке либо уже удалена.");

            return 1;
        }

        public int UpdateMeeting(MeetingDTO meeting)
        {
            if (!meeting.Id.HasValue)
                throw new Exception("Невозможно обновить данные о встрече.");

            _context.Meetings[meeting.Id.Value].MeetingStart = meeting.MeetingStart;
            _context.Meetings[meeting.Id.Value].MeetingEnd = meeting.MeetingEnd;
            _context.Meetings[meeting.Id.Value].Description = meeting.Description;

            return 1;
        }

        public int AddNewMeetingNotification(MeetingNotificationDTO notification)
        {
            var entity = new MeetingNotification
            {
                MeetingId = notification.MeetingId,
                NotificationTime = notification.NotificationTime
            };

            _context.MeetingNotifications[notification.MeetingId] = entity;

            return 1;
        }

        public int UpdateMeetingNotification(MeetingNotificationDTO notification)
        {
            _context.MeetingNotifications[notification.MeetingId].NotificationTime = notification.NotificationTime;

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

        public int RemoveMeetingNotification(int meetingId)
        {
            if (!_context.MeetingNotifications.Remove(meetingId))
                throw new Exception("Данное уведомление отсутствует в списке либо уже удалено.");

            return 1;
        }
    }
}
