using MeetingManagementApp.Domain.Models.DTO;

namespace MeetingManagementApp.Domain.Contracts
{
    public interface IMeetingController
    {
        MeetingDTO GetMeetingById(int id);

        string? ValidateMeetingSubject(string? subject);

        string? ValidateMeetingDescription(string? description);

        string? ValidateMeetingMeetingStart(DateTime meetingStart);

        string? ValidateMeetingMeetingEnd(DateTime meetingStart, DateTime meetingEnd);
    }
}
