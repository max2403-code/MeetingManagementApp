using MeetingManagementApp.Domain.Models.DTO;

namespace MeetingManagementApp.Domain.Contracts
{
    public interface IMeetingController
    {
        public IReadOnlyCollection<MeetingDTO> GetMeetingsOnDate(DateTime date);

        int AddNewMeeting(MeetingDTO meeting);

        MeetingDTO GetMeetingById(int id, DateTime? onDate = null);

        string? ValidateMeetingSubject(string? subject);

        string? ValidateMeetingDescription(string? description);

        string? ValidateMeetingMeetingStart(DateTime meetingStart, int? meetingId = null);

        string? ValidateMeetingMeetingEnd(DateTime meetingStart, DateTime meetingEnd, int? meetingId = null);
        Task<int> SaveMeetingsOnDateFileAsync(DateTime onDate, string folderPath);
        (int result, bool isNotificationDeleted) UpdateMeeting(MeetingDTO meeting);
        bool RemoveMeeting(int id);
        string? ValidateMeetingMeetingOnDate(DateTime onDate);
        bool RemoveMeetingNotification(int meetingId);





        string? ValidateMeetingNotificationTime(DateTime notificationTime, int meetingId);

        string? ValidateMeetingNotificationOnDate(DateTime onDate, int meetingId);
        int AddNewMeetingNotification(MeetingNotificationDTO notification);

        int UpdateMeetingNotification(MeetingNotificationDTO notification);
        MeetingNotificationDTO GetMeetingNotificationByMeetingId(int meetingId);

    }
}
