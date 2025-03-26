using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Models.DTO;

namespace MeetingManagementApp.Infrastructure.Controllers
{
    internal class MeetingController : IMeetingController
    {
        private readonly IMeetingService _meetingService;

        public MeetingController(IMeetingService meetingService)
        {
            _meetingService = meetingService;
        }

        public int AddNewMeeting(MeetingDTO meeting)
        {
            return _meetingService.AddNewMeeting(meeting);
        }

        public MeetingDTO GetMeetingById(int id)
        {
            return _meetingService.GetMeetingById(id);
        }

        public IReadOnlyCollection<MeetingDTO> GetMeetingsOnDate(DateTime date)
        {
            return _meetingService.GetMeetingsOnDate(date);
        }

        public string? ValidateMeetingDescription(string? description)
        {
            return _meetingService.ValidateMeetingDescription(description);
        }

        public string? ValidateMeetingMeetingEnd(DateTime meetingStart, DateTime meetingEnd)
        {
            return _meetingService.ValidateMeetingMeetingEnd(meetingStart, meetingEnd);
        }

        public string? ValidateMeetingMeetingStart(DateTime meetingStart)
        {
            return _meetingService.ValidateMeetingMeetingStart(meetingStart);
        }

        public string? ValidateMeetingSubject(string? subject)
        {
            return _meetingService.ValidateMeetingSubject(subject);
        }
    }
}
