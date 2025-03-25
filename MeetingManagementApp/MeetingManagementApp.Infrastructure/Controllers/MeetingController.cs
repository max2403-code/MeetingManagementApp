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

        public MeetingDTO GetMeetingById(int id)
        {
            return _meetingService.GetMeetingById(id);
        }
    }
}
