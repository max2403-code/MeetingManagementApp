namespace MeetingManagementApp.Domain.Exceptions
{
    public class UserInputException : Exception
    {
        public string? Value { get; }

        public UserInputException(string message, string? value = null) : base(message) 
        {
            Value = value;
        }
    }
}
