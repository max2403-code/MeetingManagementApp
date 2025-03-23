using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Models.Common;

namespace MeetingManagementApp.Infrastructure.CommandHandlers
{
    internal class MainMenuCommandHandler : ICommandRequestHandler
    {
        private readonly IReadOnlyDictionary<string, ICommandRequestHandler> _nextHandlers;
        private readonly IConsoleService _consoleService;

        public MainMenuCommandHandler(IReadOnlyDictionary<string, ICommandRequestHandler> nextHandlers, IConsoleService consoleService) 
        {
            _nextHandlers = nextHandlers;
            _consoleService = consoleService;
        }

        public CommandHandlerResult Execute<string>(string requestValue)
        {
            var consoleCommandResult = _consoleService.ExecuteOnConsole<string?>(null, GetConsoleActions);
        }

        public CommandHandlerResult Execute<T>(T requestValue)
        {
            throw new NotImplementedException();
        }

        private string GetConsoleActions(string? value)
        {
            Console.Clear();


        }
    }
}
