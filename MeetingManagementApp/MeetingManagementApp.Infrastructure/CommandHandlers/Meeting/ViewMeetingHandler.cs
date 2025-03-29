using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Exceptions;
using MeetingManagementApp.Domain.Models.Common;
using MeetingManagementApp.Domain.Models.Input;
using MeetingManagementApp.Infrastructure.AbstractHandlers;
using System.Text.Json;

namespace MeetingManagementApp.Infrastructure.CommandHandlers.Meeting
{
    internal class ViewMeetingHandler : AbstractCommandHandler
    {
        private readonly IMeetingController _meetingController;

        public ViewMeetingHandler(IPrinterService consoleService, IMeetingController meetingController) : base(consoleService)
        {
            _meetingController = meetingController;
        }

        public override string? GetCommandDescription()
        {
            return "Просмотр встречи.";
        }

        protected override CommandResult GetConsoleCommandResult(string? value)
        {
            Console.Clear();

            Console.WriteLine("Просмотр встречи");
            Console.WriteLine(new string('-', 40));

            var meeting = string.IsNullOrEmpty(value) ? new MeetingInput() : JsonSerializer.Deserialize<MeetingInput>(value) ?? new MeetingInput();

            if (meeting.IsFirstCommandCall)
                meeting = new MeetingInput
                {
                    Id = meeting.Id,
                    OnDate = meeting.OnDate,
                    IsFirstCommandCall = false
                };

            Console.WriteLine();

            var id = meeting.Id;

            if (id.HasValue)
            {
                Console.Write("Номер встречи: ");

                Console.WriteLine($"{id}");
            }
            else
            {
                Console.Write("Введите номер встречи: ");

                var idInput = Console.ReadLine();

                id = int.TryParse(idInput, out var val) ? val : throw new UserInputException("Введен некорректный номер встречи.", JsonSerializer.Serialize(meeting));
            }

            var meetingDTO = _meetingController.GetMeetingById(id.Value, meeting.OnDate) ?? throw new Exception("Невозможно открыть встречу.");

            Console.WriteLine();
            Console.WriteLine(new string('-', 40));

            Console.WriteLine();
            Console.WriteLine($"Номер встречи: {meetingDTO.Id}");

            Console.WriteLine();
            Console.WriteLine($"Заголовок: {meetingDTO.Subject}");

            Console.WriteLine();
            Console.WriteLine($"Дата: {meetingDTO.MeetingStart:dd.MM.yyyy}");

            Console.WriteLine();
            Console.WriteLine($"Начало: {meetingDTO.MeetingStart:HH:mm}");

            Console.WriteLine();
            Console.WriteLine($"Примерное окончание: {meetingDTO.MeetingEnd:HH:mm}");

            Console.WriteLine();
            Console.WriteLine($"Описание: {meetingDTO.Description}");

            Console.WriteLine();
            Console.WriteLine($"Уведомление: {(meetingDTO.MeetingNotification != null ? meetingDTO.MeetingNotification.NotificationTime.ToString("dd.MM.yyyy HH:mm") : "Отсутствует")}");
            Console.WriteLine();

            return new CommandResult
            {
                ResultValue = meeting != null ? JsonSerializer.Serialize(new MeetingInput
                {
                    Id = id,
                    MeetingStart = meetingDTO.MeetingStart,
                    MeetingNotification = meetingDTO.MeetingNotification != null ? new MeetingNotificationInput
                    {
                        MeetingId = meetingDTO.MeetingNotification.MeetingId,
                        NotificationTime = meetingDTO.MeetingNotification.NotificationTime,
                    } : null,
                    IsFirstCommandCall = true
                }) : null,
            };
        }

        protected override IReadOnlyCollection<string> GetAllowedCommands(string? requestValue)
        {
            var rval = new List<string>
            {
                "v"
            };

            var meeting = string.IsNullOrEmpty(requestValue) ? new MeetingInput() : JsonSerializer.Deserialize<MeetingInput>(requestValue) ?? new MeetingInput();

            if (!meeting.MeetingStart.HasValue)
                throw new Exception("Ошибка при просмотре встречи.");

            if (meeting.MeetingStart.Value < DateTime.Now)
                rval.Add("dm");
            else
            {
                rval.AddRange(["um", "dm"]);

                if (meeting.MeetingNotification == null)
                    rval.Add("an");
                else
                    rval.Add("vn");
            }

            rval.AddRange(["m", "q"]);

            return rval;
        }

        public override string GetCommand()
        {
            return "vm";
        }
    }
}
