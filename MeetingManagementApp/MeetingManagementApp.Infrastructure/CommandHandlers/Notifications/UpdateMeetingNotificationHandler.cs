using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Exceptions;
using MeetingManagementApp.Domain.Models.Common;
using MeetingManagementApp.Domain.Models.DTO;
using MeetingManagementApp.Domain.Models.Input;
using MeetingManagementApp.Infrastructure.AbstractHandlers;
using System.Globalization;
using System.Text.Json;

namespace MeetingManagementApp.Infrastructure.CommandHandlers.Notifications
{
    internal class UpdateMeetingNotificationHandler : AbstractCommandHandler
    {
        private readonly IMeetingController _meetingController;

        public UpdateMeetingNotificationHandler(IPrinterService consoleService, IMeetingController meetingController) : base(consoleService)
        {
            _meetingController = meetingController;
        }

        public override string GetCommand()
        {
            return "un";
        }

        public override string? GetCommandDescription()
        {
            return "Редактировать напоминание о встрече.";
        }

        protected override IReadOnlyCollection<string> GetAllowedCommands(string? requestValue)
        {
            return ["vn", "m", "q"];
        }

        protected override CommandResult GetConsoleCommandResult(string? value)
        {
            Console.Clear();

            Console.WriteLine("Редактировать напоминание о встрече");
            Console.WriteLine(new string('-', 40));

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

            if (!meeting.Id.HasValue || meeting.MeetingNotification == null)
                throw new Exception("Невозможно редактировать напоминание.");

            var meetingNotificationDTO = _meetingController.GetMeetingNotificationByMeetingId(meeting.Id.Value) ?? throw new Exception("Невозможно посмотреть существующее напоминание.");

            Console.WriteLine();

            var onDate = meeting.MeetingNotification.OnDate;

            if (onDate.HasValue)
            {
                Console.Write("Дата напоминания: ");

                Console.WriteLine($"{onDate.Value:dd.MM.yyyy}");
            }
            else
            {
                Console.WriteLine($"Текущая дата напоминания: {meetingNotificationDTO.NotificationTime:dd.MM.yyyy}");

                Console.Write("Введите дату напоминания: ");

                var onDateInput = Console.ReadLine()?.Split(".");

                onDate = onDateInput?.Length == 3 && DateTime.TryParse(string.Join("-", onDateInput[2], onDateInput[1], onDateInput[0]), CultureInfo.InvariantCulture, out var val) ? val.Date : throw new UserInputException("Введена неверная дата напоминания.", JsonSerializer.Serialize(meeting));

                var error = _meetingController.ValidateMeetingNotificationOnDate(onDate.Value, meeting.Id.Value);

                if (!string.IsNullOrEmpty(error))
                    throw new UserInputException(error, JsonSerializer.Serialize(meeting));

                meeting.MeetingNotification.OnDate = onDate;
            }

            Console.WriteLine();

            var notificationTime = meeting.MeetingNotification.NotificationTime;

            if (notificationTime.HasValue)
            {
                Console.Write("Время напоминания: ");

                Console.WriteLine($"{notificationTime.Value:HH:mm}");
            }
            else
            {
                Console.WriteLine($"Текущее время напоминания: {meetingNotificationDTO.NotificationTime:HH:mm}");

                Console.Write("Введите время напоминания: ");

                var notificationTimeInput = Console.ReadLine();
                notificationTime = DateTime.TryParse(string.Join(" ", onDate.Value.ToString("yyyy-MM-dd"), CultureInfo.InvariantCulture, notificationTimeInput + ":00"), out var val) ? val : throw new UserInputException("Введено неверное время напоминания.", JsonSerializer.Serialize(meeting));

                var error = _meetingController.ValidateMeetingNotificationTime(notificationTime.Value, meeting.Id.Value);

                if (!string.IsNullOrEmpty(error))
                    throw new UserInputException(error, JsonSerializer.Serialize(meeting));

                meeting.MeetingNotification.NotificationTime = notificationTime;
            }

            _meetingController.UpdateMeetingNotification(new MeetingNotificationDTO
            {
                MeetingId = meeting.Id.Value,
                NotificationTime = notificationTime.Value,
            });

            Console.WriteLine();

            Console.WriteLine("Напоминание обновлено.");

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
