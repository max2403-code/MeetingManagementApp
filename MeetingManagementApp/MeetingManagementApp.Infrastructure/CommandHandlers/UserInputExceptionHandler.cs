using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Models.Common;

namespace MeetingManagementApp.Infrastructure.CommandHandlers
{
    internal class UserInputExceptionHandler : ICommandRequestHandler
    {
        private readonly IPrinterService _consoleService;

        public UserInputExceptionHandler(IPrinterService consoleService)
        {
            _consoleService = consoleService;
        }

        public CommandHandlerResult Execute(string? requestValue, IReadOnlyDictionary<string, ICommandRequestHandler> handlers)
        {
            var rval = _consoleService.PrinterExecute(requestValue, GetConsoleCommandResult);

            return new CommandHandlerResult
            {
                NextCommandRequestHandler = string.IsNullOrEmpty(rval.Command) ? null : handlers["m"],
            };
        }

        public string GetCommand()
        {
            return "uex";
        }

        private CommandResult GetConsoleCommandResult(string? value)
        {
            Console.WriteLine(new string('-', 40));

            Console.WriteLine($"Ошибка: {value}");

            Console.WriteLine($"Для повтора последней операции нажмите Enter...");
            Console.WriteLine($"Для выхода в главное меню нажмите на любую клавишу...");

            var input = Console.ReadLine();

            return new CommandResult
            {
                Command = input,
            };
        }
    }
}
