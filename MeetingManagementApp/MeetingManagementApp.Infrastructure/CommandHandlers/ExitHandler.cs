using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Models.Common;

namespace MeetingManagementApp.Infrastructure.CommandHandlers
{
    internal class ExitHandler : ICommandRequestHandler
    {
        private readonly IPrinterService _consoleService;

        public ExitHandler(IPrinterService consoleService)
        {
            _consoleService = consoleService;
        }

        public CommandHandlerResult Execute(string? requestValue, IReadOnlyDictionary<string, ICommandRequestHandler> handlers, IReadOnlyCollection<(string command, string? description)> commands)
        {
            var rval = _consoleService.PrinterExecute(requestValue, GetConsoleCommandResult);

            return new CommandHandlerResult();
        }

        public string GetCommand()
        {
            return "q";
        }

        public string? GetCommandDescription()
        {
            return "Выйти из приложения.";
        }

        private CommandResult GetConsoleCommandResult(string? value)
        {
            Console.WriteLine($"Для выхода нажмите на любую клавишу...");

            Console.ReadKey();

            return new CommandResult();
        }


    }
}
