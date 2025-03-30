using MeetingManagementApp.Domain.Models.DTO;

namespace MeetingManagementApp.Domain.Contracts
{
    public interface IMeetingService
    {
        /// <summary>
        /// Получить список встреч на конкретную дату.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public IReadOnlyCollection<MeetingDTO> GetMeetingsOnDate(DateTime date);
        
        /// <summary>
        /// Добавить новую встречу.
        /// </summary>
        /// <param name="meeting"></param>
        /// <returns></returns>
        int AddNewMeeting(MeetingDTO meeting);

        /// <summary>
        /// Обновить встречу (и удалить напоминание, если начало встречи было изменено).
        /// </summary>
        /// <param name="meeting"></param>
        /// <returns></returns>
        (int result, bool isNotificationDeleted) UpdateMeeting(MeetingDTO meeting);

        /// <summary>
        /// Удалить встречу (и напоминание, если оно есть).
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool RemoveMeeting(int id);

        /// <summary>
        /// Получить встречу по ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="onDate"></param>
        /// <returns></returns>
        MeetingDTO GetMeetingById(int id, DateTime? onDate = null);

        /// <summary>
        /// Сохранить встречи на конкретную дату.
        /// </summary>
        /// <param name="onDate"></param>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        Task SaveMeetingsOnDateFileAsync(DateTime onDate, string folderPath);

        /// <summary>
        /// Сохранить информацию о встречах и напоминаниях в файл.
        /// </summary>
        /// <returns></returns>
        Task SaveStorageInfoAsync();

        /// <summary>
        /// Валидация заголовка встречи.
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        string? ValidateMeetingSubject(string? subject);

        /// <summary>
        /// Валидация описания встречи.
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        string? ValidateMeetingDescription(string? description);

        /// <summary>
        /// Валидация даты встречи.
        /// </summary>
        /// <param name="onDate"></param>
        /// <returns></returns>
        string? ValidateMeetingOnDate(DateTime onDate);

        /// <summary>
        /// Валидация времени начала встречи.
        /// </summary>
        /// <param name="meetingStart"></param>
        /// <param name="meetingId"></param>
        /// <returns></returns>
        string? ValidateMeetingStart(DateTime meetingStart, int? meetingId = null);

        /// <summary>
        /// Валидация времени примерного окончания встречи.
        /// </summary>
        /// <param name="meetingStart"></param>
        /// <param name="meetingEnd"></param>
        /// <param name="meetingId"></param>
        /// <returns></returns>
        string? ValidateMeetingEnd(DateTime meetingStart, DateTime meetingEnd, int? meetingId = null);
    }
}
