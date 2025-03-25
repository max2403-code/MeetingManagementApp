﻿using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Models.Common;

namespace MeetingManagementApp.Infrastructure.CommandHandlers
{
    internal class ExceptionHandler : ICommandRequestHandler
    {
        private readonly IPrinterService _consoleService;
        private readonly ICommandRequestHandler _mainMenuHandler;

        public ExceptionHandler(ICommandRequestHandler mainMenuHandler, IPrinterService consoleService) 
        {
            _consoleService = consoleService;
            _mainMenuHandler = mainMenuHandler;
        }

        public CommandHandlerResult Execute(string? requestValue)
        {
            var rval = _consoleService.PrinterExecute(requestValue, GetConsoleCommandResult);

            return new CommandHandlerResult
            {
                NextCommandRequestHandler = _mainMenuHandler,
            };
        }

        private CommandResult GetConsoleCommandResult(string? value)
        {
            Console.WriteLine(new string('-', 20));

            Console.WriteLine($"Ошибка: {value}");

            Console.ReadKey();

            return new CommandResult();
        }
    }
}
