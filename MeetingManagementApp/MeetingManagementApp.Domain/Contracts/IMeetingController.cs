using MeetingManagementApp.Domain.Models.DTO;

namespace MeetingManagementApp.Domain.Contracts
{
    /// <summary>
    /// Описания будут совпадать с IMeetingService и INotificationService
    /// </summary>
    public interface IMeetingController
    {
        #region Meeting

        public IReadOnlyCollection<MeetingDTO> GetMeetingsOnDate(DateTime date);

        int AddNewMeeting(MeetingDTO meeting);

        (int result, bool isNotificationDeleted) UpdateMeeting(MeetingDTO meeting);

        bool DeleteMeeting(int id);

        MeetingDTO GetMeetingById(int id, DateTime? onDate = null);
        
        Task<int> SaveMeetingsOnDateFileAsync(DateTime onDate, string folderPath);
        
        string? ValidateMeetingSubject(string? subject);

        string? ValidateMeetingDescription(string? description);

        string? ValidateMeetingMeetingStart(DateTime meetingStart, int? meetingId = null);

        string? ValidateMeetingMeetingEnd(DateTime meetingStart, DateTime meetingEnd, int? meetingId = null);

        string? ValidateMeetingMeetingOnDate(DateTime onDate);

        #endregion

        #region Meeting Notifications

        int AddNewMeetingNotification(MeetingNotificationDTO notification);

        int UpdateMeetingNotification(MeetingNotificationDTO notification);

        bool DeleteMeetingNotification(int meetingId);

        MeetingNotificationDTO GetMeetingNotificationByMeetingId(int meetingId);

        string? ValidateMeetingNotificationTime(DateTime notificationTime, int meetingId);

        string? ValidateMeetingNotificationOnDate(DateTime onDate, int meetingId);

        #endregion
    }
}
