using MeetingManagementApp.Domain.Models.Common;

namespace MeetingManagementApp.Domain.Contracts
{
    public interface ICommandRequestHandler
    {
        CommandHandlerResult Execute(string requestValue);

        string GetHandlerKey();
    }
}
