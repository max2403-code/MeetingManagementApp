using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Models.Common;

namespace MeetingManagementApp.Infrastructure.Services
{
    internal class ConsoleService : IPrinterService
    {
        private readonly object _locker = new();

        public CommandResult PrinterExecute(string? value,
            Func<string?, CommandResult> func,
            IReadOnlyDictionary<string, ICommandRequestHandler>? handlers = null,
            Func<string?, IReadOnlyCollection<string>>? allowCommandFunc = null)
        {
            lock (_locker)
            {
                var funcValue = func(value);

                if (handlers != null && handlers.Count > 0 && allowCommandFunc != null) 
                {
                    var allowCommands = allowCommandFunc(funcValue.ResultValue);

                    funcValue.Command = GetUserCommand(handlers, allowCommands);
                }
                
                return funcValue;
            }
        }

        private string? GetUserCommand(IReadOnlyDictionary<string, ICommandRequestHandler> handlers, IReadOnlyCollection<string> commands) 
        {
            Console.WriteLine(new string('-', 40));
            Console.WriteLine("Выберите команду:");

            foreach (var command in commands)
                Console.WriteLine($"{command} - {handlers[command]}");

            Console.WriteLine();

            Console.WriteLine("Команда:");

            return Console.ReadLine();
        }
    }
}
