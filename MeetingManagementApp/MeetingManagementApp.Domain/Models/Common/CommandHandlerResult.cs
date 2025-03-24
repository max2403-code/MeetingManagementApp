using MeetingManagementApp.Domain.Contracts;

namespace MeetingManagementApp.Domain.Models.Common
{
    public class CommandHandlerResult
    {
        public string? Result { get; set; }
        public ICommandRequestHandler? NextCommandRequestHandler { get; set; }
    }
}
