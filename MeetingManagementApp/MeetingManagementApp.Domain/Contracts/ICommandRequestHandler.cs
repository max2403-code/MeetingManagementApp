using MeetingManagementApp.Domain.Models.Common;

namespace MeetingManagementApp.Domain.Contracts
{
    public interface ICommandRequestHandler
    {
        CommandHandlerResult Execute(string? requestValue, IReadOnlyDictionary<string, ICommandRequestHandler> handlers, IReadOnlyCollection<(string command, string? description)> commands);

        string? GetCommandDescription() => null;

        string GetCommand();

    }
}
