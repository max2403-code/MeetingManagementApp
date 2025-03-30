using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Models.DTO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MeetingManagementApp.Infrastructure.Controllers
{
    /// <summary>
    /// Формально необходим для вызовов внутри обработчика в консоли, промежуточное звено между консолью и сервисами.
    /// </summary>
    internal class MeetingController : IMeetingController
    {
        private readonly IMeetingService _meetingService;
        private readonly INotificationService _notificationService;

        public MeetingController(IMeetingService meetingService, INotificationService notificationService)
        {
            _meetingService = meetingService;
            _notificationService = notificationService;
        }

        #region Meeting

        public IReadOnlyCollection<MeetingDTO> GetMeetingsOnDate(DateTime date)
        {
            return _meetingService.GetMeetingsOnDate(date);
        }

        public int AddNewMeeting(MeetingDTO meeting)
        {
            return _meetingService.AddNewMeeting(meeting);
        }

        public (int result, bool isNotificationDeleted) UpdateMeeting(MeetingDTO meeting)
        {
            return _meetingService.UpdateMeeting(meeting);
        }

        public bool DeleteMeeting(int id)
        {
            return _meetingService.RemoveMeeting(id);
        }

        public MeetingDTO GetMeetingById(int id, DateTime? onDate = null)
        {
            return _meetingService.GetMeetingById(id, onDate);
        }

        public async Task<int> SaveMeetingsOnDateFileAsync(DateTime onDate, string folderPath)
        {
            await _meetingService.SaveMeetingsOnDateFileAsync(onDate, folderPath);

            return 1;
        }

        public string? ValidateMeetingSubject(string? subject)
        {
            return _meetingService.ValidateMeetingSubject(subject);
        }

        public string? ValidateMeetingDescription(string? description)
        {
            return _meetingService.ValidateMeetingDescription(description);
        }

        public string? ValidateMeetingMeetingStart(DateTime meetingStart, int? meetingId = null)
        {
            return _meetingService.ValidateMeetingStart(meetingStart, meetingId);
        }

        public string? ValidateMeetingMeetingEnd(DateTime meetingStart, DateTime meetingEnd, int? meetingId = null)
        {
            return _meetingService.ValidateMeetingEnd(meetingStart, meetingEnd, meetingId);
        }

        public string? ValidateMeetingMeetingOnDate(DateTime onDate)
        {
            return _meetingService.ValidateMeetingOnDate(onDate);
        }

        #endregion

        #region Meeting Notification

        public int AddNewMeetingNotification(MeetingNotificationDTO notification)
        {
            return _notificationService.AddNewMeetingNotification(notification);
        }

        public int UpdateMeetingNotification(MeetingNotificationDTO notification)
        {
            return _notificationService.UpdateMeetingNotification(notification);
        }

        public bool DeleteMeetingNotification(int meetingId)
        {
            return _notificationService.RemoveMeetingNotification(meetingId);
        }

        public MeetingNotificationDTO GetMeetingNotificationByMeetingId(int meetingId)
        {
            return _notificationService.GetMeetingNotificationByMeetingId(meetingId);
        }

        public string? ValidateMeetingNotificationTime(DateTime notificationTime, int meetingId)
        {
            return _notificationService.ValidateMeetingNotificationTime(notificationTime, meetingId);
        }

        public string? ValidateMeetingNotificationOnDate(DateTime onDate, int meetingId)
        {
            return _notificationService.ValidateMeetingNotificationOnDate(onDate, meetingId);
        }

        #endregion
    }
}
