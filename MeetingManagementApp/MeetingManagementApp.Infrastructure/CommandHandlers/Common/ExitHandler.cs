using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Models.Common;

namespace MeetingManagementApp.Infrastructure.CommandHandlers.Common
{
    internal class ExitHandler : ICommandRequestHandler
    {
        private readonly IPrinterService _consoleService;
        private readonly IMeetingService _meetingService;

        public ExitHandler(IPrinterService consoleService, IMeetingService meetingService)
        {
            _consoleService = consoleService;
            _meetingService = meetingService;
        }

        public CommandHandlerResult Execute(string? requestValue, IReadOnlyDictionary<string, ICommandRequestHandler> handlers)
        {
            var saveStorageInfoTask = _meetingService.SaveStorageInfoAsync();

            saveStorageInfoTask.Wait();

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
