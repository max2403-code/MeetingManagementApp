namespace MeetingManagementApp.Domain.Models.Common
{
    /// <summary>
    /// Результат пользовательского ввода.
    /// </summary>
    public class CommandResult
    {
        /// <summary>
        /// Выбранная команда.
        /// </summary>
        public string? Command { get; set; }

        /// <summary>
        /// JSON параметр команды.
        /// </summary>
        public string? ResultValue { get; set; }
    }
}
