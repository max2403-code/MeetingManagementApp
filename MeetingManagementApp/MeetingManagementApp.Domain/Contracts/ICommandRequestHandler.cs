using MeetingManagementApp.Domain.Models.Common;

namespace MeetingManagementApp.Domain.Contracts
{
    public interface ICommandRequestHandler
    {
        CommandHandlerResult Execute<T>(T requestValue);

        //string GetHandlerKey();
    }
}
