using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Models.DTO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MeetingManagementApp.Infrastructure.Controllers
{
    internal class MeetingController : IMeetingController
    {
        private readonly IMeetingService _meetingService;
        private readonly INotificationService _notificationService;


        public MeetingController(IMeetingService meetingService, INotificationService notificationService)
        {
            _meetingService = meetingService;
            _notificationService = notificationService;
        }

        public int AddNewMeeting(MeetingDTO meeting)
        {
            return _meetingService.AddNewMeeting(meeting);
        }

        public MeetingDTO GetMeetingById(int id, DateTime? onDate = null)
        {
            return _meetingService.GetMeetingById(id, onDate);
        }

        public IReadOnlyCollection<MeetingDTO> GetMeetingsOnDate(DateTime date)
        {
            return _meetingService.GetMeetingsOnDate(date);
        }

        public bool RemoveMeeting(int id)
        {
            return _meetingService.RemoveMeeting(id);
        }

        public async Task<int> SaveMeetingsOnDateFileAsync(DateTime onDate, string folderPath)
        {
            await _meetingService.SaveMeetingsOnDateFileAsync(onDate, folderPath);

            return 1;
        }

        public (int result, bool isNotificationDeleted) UpdateMeeting(MeetingDTO meeting)
        {
            return _meetingService.UpdateMeeting(meeting);
        }

        public string? ValidateMeetingDescription(string? description)
        {
            return _meetingService.ValidateMeetingDescription(description);
        }

        public string? ValidateMeetingMeetingEnd(DateTime meetingStart, DateTime meetingEnd, int? meetingId = null)
        {
            return _meetingService.ValidateMeetingMeetingEnd(meetingStart, meetingEnd, meetingId);
        }

        public string? ValidateMeetingMeetingStart(DateTime meetingStart, int? meetingId = null)
        {
            return _meetingService.ValidateMeetingMeetingStart(meetingStart, meetingId);
        }

        public string? ValidateMeetingSubject(string? subject)
        {
            return _meetingService.ValidateMeetingSubject(subject);
        }

        public string? ValidateMeetingMeetingOnDate(DateTime onDate)
        {
            return _meetingService.ValidateMeetingMeetingOnDate(onDate);
        }

        public bool RemoveMeetingNotification(int meetingId)
        {
            return _notificationService.RemoveMeetingNotification(meetingId);
        }

        public string? ValidateMeetingNotificationTime(DateTime notificationTime, int meetingId)
        {
            return _notificationService.ValidateMeetingNotificationTime(notificationTime, meetingId);
        }

        public string? ValidateMeetingNotificationOnDate(DateTime onDate, int meetingId)
        {
            return _notificationService.ValidateMeetingNotificationOnDate(onDate, meetingId);
        }

        public int AddNewMeetingNotification(MeetingNotificationDTO notification)
        {
            return _notificationService.AddNewMeetingNotification(notification);
        }

        public int UpdateMeetingNotification(MeetingNotificationDTO notification)
        {
            return _notificationService.UpdateMeetingNotification(notification);
        }

        public MeetingNotificationDTO GetMeetingNotificationByMeetingId(int meetingId)
        {
            return _notificationService.GetMeetingNotificationByMeetingId(meetingId);
        }
    }
}
