using MeetingManagementApp.Domain.Models.DTO;

namespace MeetingManagementApp.Domain.Contracts
{
    public interface IMeetingController
    {
        MeetingDTO GetMeetingById(int id);
    }
}
