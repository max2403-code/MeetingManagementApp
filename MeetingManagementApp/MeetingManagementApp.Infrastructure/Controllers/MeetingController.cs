using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Models.DTO;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public MeetingDTO GetMeetingById(int id, DateTime? onDate = null)
        {
            return _meetingService.GetMeetingById(id, onDate);
        }

        public IReadOnlyCollection<MeetingDTO> GetMeetingsOnDate(DateTime date)
        {
            return _meetingService.GetMeetingsOnDate(date);
        }

        public Task SaveMeetingsOnDateFileAsync(DateTime onDate, string folderPath)
        {
            return _meetingService.SaveMeetingsOnDateFileAsync(onDate, folderPath);
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
    }
}
