namespace MeetingManagementApp.Domain.Contracts
{
    public interface INotificationSender
    {
        Task RunSender();
    }
}
