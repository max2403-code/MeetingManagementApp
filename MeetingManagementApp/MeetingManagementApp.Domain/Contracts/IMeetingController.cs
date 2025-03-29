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
        int UpdateMeeting(MeetingDTO meeting);
        bool RemoveMeeting(int id);
        string? ValidateMeetingMeetingOnDate(DateTime onDate);
    }
}
