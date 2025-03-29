using MeetingManagementApp.Domain.Models.DTO;

namespace MeetingManagementApp.Domain.Contracts
{
    public interface INotificationService
    {
        int AddNewMeetingNotification(MeetingNotificationDTO notification);

        int UpdateMeetingNotification(MeetingNotificationDTO notification);

        string? ValidateMeetingNotificationTime(DateTime notificationTime, int meetingId);

        string? ValidateMeetingNotificationOnDate(DateTime onDate, int meetingId);
       
        bool RemoveMeetingNotification(int meetingId);

        IReadOnlyCollection<MeetingNotificationDTO> GetMeetingNotifications();

        MeetingNotificationDTO GetMeetingNotificationByMeetingId(int meetingId);
    }
}
