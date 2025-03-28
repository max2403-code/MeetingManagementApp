using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Models.Common;

namespace MeetingManagementApp.Infrastructure.CommandHandlers
{
    internal class ExceptionHandler : ICommandRequestHandler
    {
        private readonly IPrinterService _consoleService;

        public ExceptionHandler(IPrinterService consoleService) 
        {
            _consoleService = consoleService;
        }

        public CommandHandlerResult Execute(string? requestValue)
        {
            var rval = _consoleService.PrinterExecute(requestValue, GetConsoleCommandResult);

            return new CommandHandlerResult();
        }

        public string GetCommand()
        {
            return "ex";
        }

        private CommandResult GetConsoleCommandResult(string? value)
        {
            Console.WriteLine(new string('-', 20));

            Console.WriteLine($"Ошибка: {value}");

            Console.WriteLine($"Нажмите на любую клавишу...");

            Console.ReadKey();

            return new CommandResult();
        }
    }
}
