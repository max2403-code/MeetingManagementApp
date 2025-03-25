using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Models.Common;
using MeetingManagementApp.Infrastructure.AbstractHandlers;

namespace MeetingManagementApp.Infrastructure.CommandHandlers
{
    internal class ExceptionHandler : AbstractCommandHandler
    {
        public ExceptionHandler(IReadOnlyDictionary<string, ICommandRequestHandler> nextHandlers, IPrinterService consoleService) : base(nextHandlers, consoleService)
        {
        }

        public override string? GetCommandDescription()
        {
            return null;
        }

        protected override IReadOnlyDictionary<string, string?> GetAllowedCommandsMap(string? requestValue)
        {
            return _nextHandlers.ToDictionary(k => k.Key, v => v.Value.GetCommandDescription());
        }

        protected override CommandResult? GetConsoleCommandResult(string? value)
        {
            Console.WriteLine(new string('-', 20));

            Console.WriteLine($"Ошибка: {value}");

            return new CommandResult
            {
                Command = "m"
            };
        }
    }
}
