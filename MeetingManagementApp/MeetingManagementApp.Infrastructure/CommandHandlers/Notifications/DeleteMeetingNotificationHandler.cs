using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Models.Common;
using MeetingManagementApp.Domain.Models.Input;
using MeetingManagementApp.Infrastructure.AbstractHandlers;
using System.Text.Json;

namespace MeetingManagementApp.Infrastructure.CommandHandlers.Notifications
{
    internal class DeleteMeetingNotificationHandler : AbstractCommandHandler
    {
        private readonly IMeetingController _meetingController;

        public DeleteMeetingNotificationHandler(IPrinterService consoleService, IMeetingController meetingController) : base(consoleService)
        {
            _meetingController = meetingController;
        }

        public override string GetCommand()
        {
            return "dn";
        }

        public override string? GetCommandDescription()
        {
            return "Удалить напоминание о встрече.";
        }

        protected override IReadOnlyCollection<string> GetAllowedCommands(string? requestValue)
        {
            return ["vm", "m", "q"];
        }

        protected override CommandResult GetConsoleCommandResult(string? value)
        {
            var newMeeting = new MeetingInput
            {
                MeetingNotification = new MeetingNotificationInput()
            };

            var meeting = string.IsNullOrEmpty(value) ? newMeeting : JsonSerializer.Deserialize<MeetingInput>(value) ?? newMeeting;

            if (meeting.IsFirstCommandCall)
                meeting = new MeetingInput
                {
                    Id = meeting.Id,
                    MeetingNotification = new MeetingNotificationInput
                    {
                        MeetingId = meeting.Id,
                    },
                    IsFirstCommandCall = false
                };

            if (!meeting.Id.HasValue)
                throw new Exception("Невозможно удалить напоминание.");

            var result = _meetingController.DeleteMeetingNotification(meeting.Id.Value);

            Console.WriteLine();

            if (result)
                Console.WriteLine("Напоминание успешно удалено.");
            else
                Console.WriteLine("Напоминание не удалено.");

            return new CommandResult
            {
                ResultValue = meeting != null ? JsonSerializer.Serialize(new MeetingInput
                {
                    Id = meeting.Id,
                    IsFirstCommandCall = true
                }) : null,
            };
        }
    }
}
