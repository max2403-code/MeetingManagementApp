namespace MeetingManagementApp.Storage.Entities
{
    public class Meeting
    {
        /// <summary>
        /// ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Начало встречи.
        /// </summary>
        public DateTime MeetingStart { get; set; }

        /// <summary>
        /// Окончание встречи.
        /// </summary>
        public DateTime MeetingEnd { get; set; }

        /// <summary>
        /// Описание встречи.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Заголовок встречи.
        /// </summary>
        public string Subject { get; set; }

    }
}
