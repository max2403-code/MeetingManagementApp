namespace MeetingManagementApp.Domain.Contracts
{
    public interface IConsoleService
    {
        string ExecuteOnConsole<T>(T value, Func<T, string> func);
    }
}
