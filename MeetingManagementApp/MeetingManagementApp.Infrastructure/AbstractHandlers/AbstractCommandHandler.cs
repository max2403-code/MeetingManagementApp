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

        public CommandHandlerResult Execute(string? requestValue, IReadOnlyDictionary<string, ICommandRequestHandler> handlers)
        {
            var consoleCommandResult = _printerService.PrinterExecute(requestValue, GetConsoleCommandResult, handlers, GetAllowedCommands);

            var allowedCommands = GetAllowedCommands(consoleCommandResult.ResultValue);

            if (string.IsNullOrEmpty(consoleCommandResult.Command) || // Если нет команды
                !handlers.ContainsKey(consoleCommandResult.Command) || // или команда незнакомая
                !allowedCommands.Contains(consoleCommandResult.Command)) // или команда недоступная
                throw new Exception("Неверная команда.");

            return new CommandHandlerResult
            {
                NextCommandRequestHandler = handlers.TryGetValue(consoleCommandResult.Command, out var handler) ? handler : throw new Exception($"Обработчик команды \"{consoleCommandResult.Command}\" отсутствует"),
                Result = consoleCommandResult.ResultValue
            };
        }

        /// <summary>
        /// Выполнение инструкций внутри команды.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected abstract CommandResult GetConsoleCommandResult(string? value);

        /// <summary>
        /// Получить список наименований доступных команд в консоли.
        /// </summary>
        /// <param name="requestValue"></param>
        /// <returns></returns>
        protected abstract IReadOnlyCollection<string> GetAllowedCommands(string? requestValue);

        public abstract string? GetCommandDescription();

        public abstract string GetCommand();
    }
}
