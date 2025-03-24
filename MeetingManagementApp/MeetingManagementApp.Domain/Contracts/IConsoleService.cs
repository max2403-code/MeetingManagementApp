using MeetingManagementApp.Domain.Models.Common;

namespace MeetingManagementApp.Domain.Contracts
{
    public interface IConsoleService
    {
        ConsoleCommandResult? ExecuteOnConsole(string? value, Func<string?, ConsoleCommandResult?> func, Func<string?, IReadOnlyDictionary<string, string>> allowCommandFunc);
    }
}
