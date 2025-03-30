using MeetingManagementApp.Domain.Models.DTO;

namespace MeetingManagementApp.Domain.Contracts
{
    public interface INotificationService
    {
        /// <summary>
        /// Добавить новое напоминание.
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        int AddNewMeetingNotification(MeetingNotificationDTO notification);

        /// <summary>
        /// Редактировать напоминание.
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        int UpdateMeetingNotification(MeetingNotificationDTO notification);
       
        /// <summary>
        /// Удалить напоминание.
        /// </summary>
        /// <param name="meetingId"></param>
        /// <returns></returns>
        bool RemoveMeetingNotification(int meetingId);

        /// <summary>
        /// Получить напоминание по ID встречи.
        /// </summary>
        /// <param name="meetingId"></param>
        /// <returns></returns>
        MeetingNotificationDTO GetMeetingNotificationByMeetingId(int meetingId);

        /// <summary>
        /// Получить напоминания для отправки.
        /// </summary>
        /// <returns></returns>
        IReadOnlyCollection<MeetingNotificationDTO> GetMeetingNotificationsWithTimeRange();

        /// <summary>
        /// Валидация времени напоминания.
        /// </summary>
        /// <param name="notificationTime"></param>
        /// <param name="meetingId"></param>
        /// <returns></returns>
        string? ValidateMeetingNotificationTime(DateTime notificationTime, int meetingId);

        /// <summary>
        /// Валидация даты напоминания.
        /// </summary>
        /// <param name="onDate"></param>
        /// <param name="meetingId"></param>
        /// <returns></returns>
        string? ValidateMeetingNotificationOnDate(DateTime onDate, int meetingId);
    }
}
