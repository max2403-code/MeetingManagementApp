using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Models.Common;

namespace MeetingManagementApp.Infrastructure.AbstractHandlers
{
    // Наследуются только те обработчики, которые содержат внутри себя следующие обработчики
    internal abstract class AbstractCommandHandler : ICommandRequestHandler
    {
        private protected readonly IReadOnlyDictionary<string, ICommandRequestHandler> _nextHandlers;
        private readonly IPrinterService _printerService;
        private readonly IReadOnlyCollection<(string command, string? description)> _nextCommands;


        public AbstractCommandHandler(IReadOnlyDictionary<string, ICommandRequestHandler> nextHandlers, IPrinterService consoleService) 
        {
            _nextHandlers = nextHandlers;
            _printerService = consoleService;
            _nextCommands = _nextHandlers.Select(x => (x.Key, x.Value.GetCommandDescription())).ToArray();
        }

        public CommandHandlerResult Execute(string? requestValue)
        {
            var consoleCommandResult = _printerService.PrinterExecute(requestValue, GetConsoleCommandResult, _nextCommands, GetNotAllowedCommands);

            var notAllowedCommands = GetNotAllowedCommands(consoleCommandResult.ResultValue);

            if ((string.IsNullOrEmpty(consoleCommandResult.Command) || // Если нет команды
                !_nextHandlers.ContainsKey(consoleCommandResult.Command) || // или есть незнакомая команда
                notAllowedCommands != null && notAllowedCommands.Contains(consoleCommandResult.Command))) // или команда знакомая, но недоступная в данном контексте
                throw new Exception("Неверная команда.");

            return new CommandHandlerResult
            {
                NextCommandRequestHandler = _nextHandlers[consoleCommandResult.Command],
                Result = consoleCommandResult.ResultValue
            };
        }

        protected abstract CommandResult GetConsoleCommandResult(string? value);

        protected abstract ISet<string>? GetNotAllowedCommands(string? requestValue);

        public abstract string? GetCommandDescription();
    }
}
