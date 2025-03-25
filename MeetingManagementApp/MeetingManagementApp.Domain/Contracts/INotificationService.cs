using MeetingManagementApp.Domain.Models.DTO;

namespace MeetingManagementApp.Domain.Contracts
{
    public interface INotificationService
    {
        IReadOnlyCollection<MeetingNotificationDTO> GetMeetingNotifications();
    }
}
