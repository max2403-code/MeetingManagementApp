using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Models.Common;
using MeetingManagementApp.Domain.Models.Input;
using MeetingManagementApp.Infrastructure.AbstractHandlers;
using System.Text.Json;

namespace MeetingManagementApp.Infrastructure.CommandHandlers
{
    internal class ViewMeetingNotificationHandler : AbstractCommandHandler
    {
        private readonly IMeetingController _meetingController;

        public ViewMeetingNotificationHandler(IPrinterService consoleService, IMeetingController meetingController) : base(consoleService)
        {
            _meetingController = meetingController;
        }

        public override string GetCommand()
        {
            return "vn";
        }

        public override string? GetCommandDescription()
        {
            return "Посмотреть напоминание.";
        }

        protected override IReadOnlyCollection<string> GetAllowedCommands(string? requestValue)
        {
            return ["un", "dn", "vm", "m", "q"];
        }

        protected override CommandResult GetConsoleCommandResult(string? value)
        {
            Console.Clear();

            Console.WriteLine("Просмотр напоминания");
            Console.WriteLine(new string('-', 40));

            var meeting = string.IsNullOrEmpty(value) ? new MeetingInput() : JsonSerializer.Deserialize<MeetingInput>(value) ?? new MeetingInput();

            if (meeting.IsFirstCommandCall)
                meeting = new MeetingInput
                {
                    Id = meeting.Id,
                    IsFirstCommandCall = false
                };

            if (!meeting.Id.HasValue)
                throw new Exception("Невозможно посмотреть напоминание.");

            Console.WriteLine();

            var meetingNotificationDTO = _meetingController.GetMeetingNotificationByMeetingId(meeting.Id.Value) ?? throw new Exception("Невозможно посмотреть напоминание.");

            Console.WriteLine();
            Console.WriteLine($"Номер встречи: {meeting.Id.Value}");

            Console.WriteLine();
            Console.WriteLine($"Дата: {meetingNotificationDTO.NotificationTime:dd.MM.yyyy}");

            Console.WriteLine();
            Console.WriteLine($"Время: {meetingNotificationDTO.NotificationTime:HH:mm}");
            Console.WriteLine();

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
