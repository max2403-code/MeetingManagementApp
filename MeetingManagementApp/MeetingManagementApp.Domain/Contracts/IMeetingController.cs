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

        string? ValidateMeetingMeetingStart(DateTime meetingStart);

        string? ValidateMeetingMeetingEnd(DateTime meetingStart, DateTime meetingEnd);
    }
}
