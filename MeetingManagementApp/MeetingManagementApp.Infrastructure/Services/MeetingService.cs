﻿using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Models.DTO;
using MeetingManagementApp.Storage.Context;
using MeetingManagementApp.Storage.Entities;

namespace MeetingManagementApp.Infrastructure.Services
{
    internal class MeetingService : IMeetingService
    {
        private readonly MeetingStorageContext _context;
        private readonly INotificationService _notificationService;

        public MeetingService(MeetingStorageContext context, INotificationService notificationService) 
        {
            _context = context;
            _notificationService = notificationService;
        }

        public IReadOnlyCollection<MeetingDTO> GetMeetingsOnDate(DateTime date)
        {
            var dateFrom = date.Date;
            var dateTo = dateFrom.AddDays(1);

            return _context.Meetings.Values
                .Where(x => x.MeetingStart >= dateFrom && x.MeetingStart < dateTo)
                .OrderBy(x => x.MeetingStart)
                .Select(x => new MeetingDTO
                {
                    Id = x.Id,
                    MeetingStart = x.MeetingStart,
                    MeetingEnd = x.MeetingEnd,
                    Description = x.Description,
                    Subject = x.Subject,
                    MeetingNotification = _context.MeetingNotifications.TryGetValue(x.Id, out var val) ? new MeetingNotificationDTO
                    {
                        MeetingId = val.MeetingId,
                        NotificationTime = val.NotificationTime
                    } : null
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
                Description = meeting.Description,
                Subject = meeting.Subject
            };

            _context.Meetings[entityIndex] = entity;

            return entityIndex;
        }

        public (int result, bool isNotificationDeleted) UpdateMeeting(MeetingDTO meeting)
        {
            if (!meeting.Id.HasValue)
                throw new Exception("Невозможно обновить данные о встрече.");

            var isNotificationDeleted = false;

            var currentMeetingStart = _context.Meetings[meeting.Id.Value].MeetingStart;

            _context.Meetings[meeting.Id.Value].MeetingStart = meeting.MeetingStart;
            _context.Meetings[meeting.Id.Value].MeetingEnd = meeting.MeetingEnd;
            _context.Meetings[meeting.Id.Value].Description = meeting.Description;
            _context.Meetings[meeting.Id.Value].Subject = meeting.Subject;

            if (currentMeetingStart != meeting.MeetingStart)
                isNotificationDeleted = _notificationService.RemoveMeetingNotification(meeting.Id.Value);

            return (1, isNotificationDeleted);
        }

        public bool RemoveMeeting(int id) 
        {
            var result = _context.Meetings.Remove(id);

            _notificationService.RemoveMeetingNotification(id);

            return result;
        }

        public MeetingDTO GetMeetingById(int id, DateTime? onDate = null)
        {
             MeetingDTO? meeting;

            if (onDate.HasValue)
            {
                meeting = GetMeetingsOnDate(onDate.Value).FirstOrDefault(x => x.Id == id);

                if (meeting == null)
                    throw new Exception($"Данная встреча отсутствует на дату {onDate:dd.MM/yyyy}.");
            }
            else
            {
                if (!_context.Meetings.TryGetValue(id, out var entity))
                    throw new Exception($"Данная встреча отсутствует.");

                meeting = new MeetingDTO
                {
                    Id = entity.Id,
                    Description = entity.Description,
                    Subject = entity.Subject,
                    MeetingStart = entity.MeetingStart,
                    MeetingEnd = entity.MeetingEnd,
                    MeetingNotification = _context.MeetingNotifications.TryGetValue(id, out var val) ? new MeetingNotificationDTO
                    {
                        MeetingId = val.MeetingId,
                        NotificationTime = val.NotificationTime
                    } : null
                };
            }

            return meeting;
        }

        public async Task SaveMeetingsOnDateFileAsync(DateTime onDate, string folderPath)
        {
            var items = GetMeetingsOnDate(onDate);

            var fullPath = Path.Combine(folderPath, $"Встречи за {onDate:dd.MM.yyyy}.txt");

            using var streamWriter = new StreamWriter(fullPath, false);

            await streamWriter.WriteLineAsync($"Список встреч за {onDate:dd.MM.yyyy}:");

            if (items.Count == 0)
            {
                await streamWriter.WriteLineAsync();
                await streamWriter.WriteAsync("Встречи на данную дату отсутствуют.");
                return;
            }

            foreach (var item in items)
            {
                await streamWriter.WriteLineAsync();
                await streamWriter.WriteLineAsync(new string('-', 30));

                await streamWriter.WriteLineAsync();
                await streamWriter.WriteLineAsync($"Заголовок: {item.Subject}");

                await streamWriter.WriteLineAsync();
                await streamWriter.WriteLineAsync($"Начало: {item.MeetingStart:HH:mm}");

                await streamWriter.WriteLineAsync();
                await streamWriter.WriteLineAsync($"Примерное окончание: {item.MeetingEnd:HH:mm}");

                await streamWriter.WriteLineAsync();
                await streamWriter.WriteLineAsync($"Описание: {item.Description}");
            }
        }

        public async Task SaveStorageInfoAsync()
        {
            var t1 = _context.SaveMeetingStorageInfoAsync();
            var t2 = _context.SaveMeetingNotificationsStorageInfoAsync();

            await Task.WhenAll(t1, t2);
        }

        public string? ValidateMeetingSubject(string? subject)
        {
            if (string.IsNullOrEmpty(subject))
                return "Заголовок встречи должен быть заполнен.";
            return null;
        }

        public string? ValidateMeetingDescription(string? description)
        {
            if (string.IsNullOrEmpty(description))
                return "Заголовок встречи должен быть заполнен.";
            return null;
        }

        public string? ValidateMeetingOnDate(DateTime onDate)
        {
            if (onDate.Date < DateTime.Today)
                return "Встречу можно планировать только в будущем.";
            return null;
        }

        public string? ValidateMeetingStart(DateTime meetingStart, int? meetingId = null)
        {
            if (meetingStart < DateTime.Now)
                return "Встречу можно планировать только в будущем.";

            var isMeetingExist = meetingId.HasValue
                ? _context.Meetings.Values.Where(x => x.Id != meetingId.Value).Any(x => x.MeetingStart <= meetingStart && x.MeetingEnd >= meetingStart)
                : _context.Meetings.Values.Any(x => x.MeetingStart <= meetingStart && x.MeetingEnd >= meetingStart);

            if (isMeetingExist)
                return "На данный промежуток времени уже запланирована другая встреча.";

            return null;
        }

        public string? ValidateMeetingEnd(DateTime meetingStart, DateTime meetingEnd, int? meetingId = null)
        {
            if (meetingEnd <= meetingStart)
                return "Окончание встречи не может быть равно или раньше ее начала.";

            var isMeetingExist = meetingId.HasValue
                ? _context.Meetings.Values.Where(x => x.Id != meetingId.Value).Any(x =>
                x.MeetingStart <= meetingEnd && x.MeetingEnd >= meetingEnd ||
                x.MeetingStart >= meetingStart && x.MeetingEnd <= meetingEnd)
                : _context.Meetings.Values.Any(x =>
                x.MeetingStart <= meetingEnd && x.MeetingEnd >= meetingEnd ||
                x.MeetingStart >= meetingStart && x.MeetingEnd <= meetingEnd);

            if (isMeetingExist)
                return "На данный промежуток времени уже запланирована другая встреча.";

            return null;
        }
    }
}
