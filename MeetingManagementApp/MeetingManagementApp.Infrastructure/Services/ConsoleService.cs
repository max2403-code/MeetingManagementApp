using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Models.Common;

namespace MeetingManagementApp.Infrastructure.Services
{
    internal class ConsoleService : IConsoleService
    {
        private readonly object _locker = new();

        public ConsoleCommandResult? ExecuteOnConsole(string? value, Func<string?, ConsoleCommandResult?> func, Func<string?, IReadOnlyDictionary<string, string>> allowCommandFunc)
        {
            lock (_locker)
            {
                var funcValue = func(value);

                if (funcValue != null)
                {
                    var allowCommands = allowCommandFunc(funcValue.ResultValue).Select(x => string.Join(" - ", x.Key, x.Value)).ToArray();
                    funcValue.Command = GetUserCommand(allowCommands);
                }

                return funcValue;
            }
        }

        public string? GetUserCommand(IReadOnlyCollection<string> commands) 
        {
            Console.WriteLine(new string('-', 20));
            Console.WriteLine("Выберите команду:");

            foreach (var command in commands)
                Console.WriteLine(command);

            return Console.ReadLine();
        }
    }
}
