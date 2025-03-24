namespace MeetingManagementApp.Domain.Exceptions
{
    public class BusinessException : Exception
    {
        public string Value { get; }

        public BusinessException(string message, string value) : base(message) 
        {
            Value = value;
        }
    }
}
