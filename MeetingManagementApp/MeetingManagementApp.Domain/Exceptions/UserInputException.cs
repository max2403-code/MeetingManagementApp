namespace MeetingManagementApp.Domain.Exceptions
{
    /// <summary>
    /// Отлавливает ошибки ввода пользователя.
    /// </summary>
    public class UserInputException : Exception
    {
        /// <summary>
        /// JSON параметр команды.
        /// </summary>
        public string? Value { get; }

        public UserInputException(string message, string? value = null) : base(message) 
        {
            Value = value;
        }
    }
}
