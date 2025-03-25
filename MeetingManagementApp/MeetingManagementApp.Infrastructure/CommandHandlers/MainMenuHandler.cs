using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Models.Common;
using MeetingManagementApp.Infrastructure.AbstractHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingManagementApp.Infrastructure.CommandHandlers
{
    internal class MainMenuHandler : AbstractCommandHandler
    {
        public MainMenuHandler(IReadOnlyDictionary<string, ICommandRequestHandler> nextHandlers, IPrinterService consoleService) : base(nextHandlers, consoleService)
        {
        }

        public override string? GetCommandDescription()
        {
            return "Главное меню";
        }

        protected override CommandResult GetConsoleCommandResult(string? value)
        {
            Console.Clear();

            return new CommandResult();
        }

        protected override ISet<string>? GetNotAllowedCommands(string? requestValue)
        {
            return null;
        }
    }
}
