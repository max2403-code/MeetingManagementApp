using MeetingManagementApp.Domain.Models.DTO;

namespace MeetingManagementApp.Domain.Contracts
{
    public interface IMeetingService
    {
        public IReadOnlyCollection<MeetingDTO> GetMeetingsOnDate(DateTime date);
        
        int AddNewMeeting(MeetingDTO meeting);

        string? ValidateMeetingSubject(string? subject);

        string? ValidateMeetingDescription(string? description);

        string? ValidateMeetingMeetingStart(DateTime meetingStart);

        string? ValidateMeetingMeetingEnd(DateTime meetingStart, DateTime meetingEnd);

        int RemoveMeeting(int id);

        int UpdateMeeting(MeetingDTO meeting);

        MeetingDTO GetMeetingById(int id, DateTime? onDate = null);

        Task SaveMeetingsOnDateFileAsync(DateTime onDate, string folderPath);
    }
}
