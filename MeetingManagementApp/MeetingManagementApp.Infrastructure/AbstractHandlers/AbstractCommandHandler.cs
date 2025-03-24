﻿using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Models.Common;

namespace MeetingManagementApp.Infrastructure.AbstractHandlers
{
    internal abstract class AbstractCommandHandler : ICommandRequestHandler
    {
        private readonly IReadOnlyDictionary<string, ICommandRequestHandler> _nextHandlers;
        private readonly IConsoleService _consoleService;


        public AbstractCommandHandler(IReadOnlyDictionary<string, ICommandRequestHandler> nextHandlers, IConsoleService consoleService) 
        {
            _nextHandlers = nextHandlers;
            _consoleService = consoleService;
        }

        public CommandHandlerResult? Execute(string? requestValue)
        {
            var consoleCommandResult = _consoleService.ExecuteOnConsole(requestValue, GetConsoleCommandResult, GetAllowedCommands);

            if (consoleCommandResult == null)
                return null;

            var allowedCommands = GetAllowedCommands(consoleCommandResult.ResultValue);

            if (string.IsNullOrEmpty(consoleCommandResult.Command) || allowedCommands.ContainsKey(consoleCommandResult.Command))
                throw new Exception("Неверная команда.");

            return new CommandHandlerResult
            {
                NextCommandRequestHandler = _nextHandlers[consoleCommandResult.Command],
                Result = consoleCommandResult.ResultValue
            };
        }

        protected abstract ConsoleCommandResult? GetConsoleCommandResult(string? value);

        protected abstract IReadOnlyDictionary<string, string> GetAllowedCommands(string? requestValue);

        public abstract string GetCommandDescription();
    }
}
