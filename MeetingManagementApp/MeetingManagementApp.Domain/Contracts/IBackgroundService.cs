namespace MeetingManagementApp.Domain.Contracts
{
    public interface IBackgroundService
    {
        /// <summary>
        /// Запуск фонового процесса.
        /// </summary>
        void Run();
    }
}
