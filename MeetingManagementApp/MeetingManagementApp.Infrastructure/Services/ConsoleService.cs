using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Models.Common;

namespace MeetingManagementApp.Infrastructure.Services
{
    internal class ConsoleService : IPrinterService
    {
        private readonly object _locker = new();

        public CommandResult PrinterExecute(string? value,
            Func<string?, CommandResult> func,
            IReadOnlyCollection<(string command, string? description)>? commands = null,
            Func<string?, ISet<string>>? allowCommandFunc = null)
        {
            lock (_locker)
            {
                var funcValue = func(value);

                if (commands != null && commands.Count > 0 && allowCommandFunc != null) 
                {
                    var allowCommandsSet = allowCommandFunc(funcValue.ResultValue);

                    if (allowCommandsSet.Count > 0)
                        commands = commands.Where(x => allowCommandsSet.Contains(x.command)).Select(x => (x.command, x.description)).ToArray();

                    funcValue.Command = GetUserCommand(commands);
                }
                
                return funcValue;
            }
        }

        private string? GetUserCommand(IReadOnlyCollection<(string command, string? description)> commands) 
        {
            Console.WriteLine(new string('-', 20));
            Console.WriteLine("Выберите команду:");

            foreach (var command in commands)
                Console.WriteLine($"{command.command} - {command.description}");

            Console.WriteLine("Команда:");

            return Console.ReadLine();
        }
    }
}
