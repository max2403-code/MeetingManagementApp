namespace MeetingManagementApp.Domain.Contracts
{
    public interface IConsoleService
    {
        string ExecuteOnConsole(string value, Func<string, string> func);
    }
}
