using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Models.Common;
using MeetingManagementApp.Infrastructure.AbstractHandlers;

namespace MeetingManagementApp.Infrastructure.CommandHandlers
{
    internal class MainMenuHandler : AbstractCommandHandler
    {
        public MainMenuHandler(IEnumerable<ICommandRequestHandler> nextHandlers, IPrinterService consoleService) : base(nextHandlers, consoleService)
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

        protected override ISet<string> GetAllowedCommands(string? requestValue)
        {
            return new HashSet<string>(["v", "am", "q"]);
        }

        public override string GetCommand()
        {
            return "q";
        }
    }
}
