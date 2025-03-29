using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Models.Common;
using MeetingManagementApp.Infrastructure.AbstractHandlers;

namespace MeetingManagementApp.Infrastructure.CommandHandlers.Common
{
    internal class MainMenuHandler : AbstractCommandHandler
    {
        public MainMenuHandler(IPrinterService consoleService) : base(consoleService)
        {
        }

        public override string? GetCommandDescription()
        {
            return "Главное меню.";
        }

        protected override CommandResult GetConsoleCommandResult(string? value)
        {
            Console.Clear();

            Console.WriteLine("Главное меню");

            return new CommandResult();
        }

        protected override IReadOnlyCollection<string> GetAllowedCommands(string? requestValue)
        {
            return ["v", "am", "q"];
        }

        public override string GetCommand()
        {
            return "m";
        }
    }
}
