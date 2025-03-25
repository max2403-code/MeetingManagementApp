using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Models.Common;

namespace MeetingManagementApp.Infrastructure.Services
{
    internal class ConsoleService : IPrinterService
    {
        private readonly object _locker = new();

        public CommandResult? PrinterExecute(string? value, Func<string?, CommandResult?> func, Func<string?, IReadOnlyDictionary<string, string>>? allowCommandFunc = null)
        {
            lock (_locker)
            {
                var funcValue = func(value);

                if (funcValue != null && allowCommandFunc != null)
                {
                    var allowCommands = allowCommandFunc(funcValue.ResultValue).Select(x => string.Join(" - ", x.Key, x.Value)).ToArray();
                    funcValue.Command = GetUserCommand(allowCommands);
                }

                return funcValue;
            }
        }

        private string? GetUserCommand(IReadOnlyCollection<string> commands) 
        {
            Console.WriteLine(new string('-', 20));
            Console.WriteLine("Выберите команду:");

            foreach (var command in commands)
                Console.WriteLine(command);

            Console.WriteLine("Команда:");

            return Console.ReadLine();
        }
    }
}
