﻿using MeetingManagementApp.Domain.Contracts;
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
                NextCommandRequestHandler = handlers[consoleCommandResult.Command],
                Result = consoleCommandResult.ResultValue
            };
        }

        protected abstract CommandResult GetConsoleCommandResult(string? value);

        protected abstract IReadOnlyCollection<string> GetAllowedCommands(string? requestValue);

        public abstract string? GetCommandDescription();

        public abstract string GetCommand();
    }
}
