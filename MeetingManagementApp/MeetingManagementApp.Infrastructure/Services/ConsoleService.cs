using MeetingManagementApp.Domain.Contracts;

namespace MeetingManagementApp.Infrastructure.Services
{
    internal class ConsoleService : IConsoleService
    {
        private readonly object _locker = new();

        public string ExecuteOnConsole(string value, Func<string, string> func)
        {
            lock (_locker)
            {
                return func(value);
            }
        }
    }
}
