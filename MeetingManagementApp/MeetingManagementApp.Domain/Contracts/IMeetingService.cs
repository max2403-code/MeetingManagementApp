using MeetingManagementApp.Domain.Models.DTO;

namespace MeetingManagementApp.Domain.Contracts
{
    public interface IMeetingService
    {
        public IReadOnlyCollection<MeetingDTO> GetMeetingsOnDate(DateTime date);
        
        int AddNewMeeting(MeetingDTO meeting);

        string? ValidateMeetingSubject(string? subject);

        string? ValidateMeetingDescription(string? description);

        string? ValidateMeetingMeetingOnDate(DateTime onDate);

        string? ValidateMeetingMeetingStart(DateTime meetingStart, int? meetingId = null);

        string? ValidateMeetingMeetingEnd(DateTime meetingStart, DateTime meetingEnd, int? meetingId = null);

        bool RemoveMeeting(int id);

        (int result, bool isNotificationDeleted) UpdateMeeting(MeetingDTO meeting);

        MeetingDTO GetMeetingById(int id, DateTime? onDate = null);

        Task SaveMeetingsOnDateFileAsync(DateTime onDate, string folderPath);

        Task SaveStorageInfoAsync();
    }
}
