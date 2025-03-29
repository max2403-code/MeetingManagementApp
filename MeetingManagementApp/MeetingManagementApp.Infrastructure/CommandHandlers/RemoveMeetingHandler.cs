using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Models.Common;
using MeetingManagementApp.Domain.Models.Input;
using MeetingManagementApp.Infrastructure.AbstractHandlers;
using System.Text.Json;

namespace MeetingManagementApp.Infrastructure.CommandHandlers
{
    internal class RemoveMeetingHandler : AbstractCommandHandler
    {
        private readonly IMeetingController _meetingController;

        public RemoveMeetingHandler(IPrinterService consoleService, IMeetingController meetingController) : base(consoleService)
        {
            _meetingController = meetingController;
        }

        public override string? GetCommandDescription()
        {
            return "Удалить встречу.";
        }

        protected override IReadOnlyCollection<string> GetAllowedCommands(string? requestValue)
        {
            return ["m", "q"];
        }

        protected override CommandResult GetConsoleCommandResult(string? value)
        {
            Console.WriteLine();

            var meeting = string.IsNullOrEmpty(value) ? new MeetingInput() : JsonSerializer.Deserialize<MeetingInput>(value) ?? new MeetingInput();

            if (meeting.IsFirstCommandCall)
                meeting = new MeetingInput
                {
                    Id = meeting.Id,
                    IsFirstCommandCall = false
                };

            if (!meeting.Id.HasValue)
                throw new Exception("Невозможно удалить встречу.");

            var result = _meetingController.RemoveMeeting(meeting.Id.Value);

            if (result)
                Console.WriteLine("Встреча успешно удалена.");
            else 
                Console.WriteLine("Встреча не удалена.");

            return new CommandResult();
        }

        public override string GetCommand()
        {
            return "dm";
        }
    }
}
