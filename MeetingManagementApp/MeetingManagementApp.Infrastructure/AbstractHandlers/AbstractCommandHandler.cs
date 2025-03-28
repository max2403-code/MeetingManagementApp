using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Models.Common;

namespace MeetingManagementApp.Infrastructure.AbstractHandlers
{
    // Наследуются только те обработчики, которые содержат внутри себя следующие обработчики
    internal abstract class AbstractCommandHandler : ICommandRequestHandler
    {
        private readonly IPrinterService _printerService;

        public AbstractCommandHandler(IPrinterService consoleService) 
        {
            _printerService = consoleService;
        }

        public CommandHandlerResult Execute(string? requestValue, IReadOnlyDictionary<string, ICommandRequestHandler> handlers, IReadOnlyCollection<(string command, string? description)> commands)
        {
            var consoleCommandResult = _printerService.PrinterExecute(requestValue, GetConsoleCommandResult, commands, GetAllowedCommands);

            var allowedCommands = GetAllowedCommands(consoleCommandResult.ResultValue);

            if (string.IsNullOrEmpty(consoleCommandResult.Command) || // Если нет команды
                !handlers.ContainsKey(consoleCommandResult.Command) || 
                !allowedCommands.Contains(consoleCommandResult.Command)) // или команда незнакомая
                throw new Exception("Неверная команда.");

            return new CommandHandlerResult
            {
                NextCommandRequestHandler = handlers[consoleCommandResult.Command],
                Result = consoleCommandResult.ResultValue
            };
        }

        protected abstract CommandResult GetConsoleCommandResult(string? value);

        protected abstract ISet<string> GetAllowedCommands(string? requestValue);

        public abstract string? GetCommandDescription();

        public abstract string GetCommand();
    }
}
