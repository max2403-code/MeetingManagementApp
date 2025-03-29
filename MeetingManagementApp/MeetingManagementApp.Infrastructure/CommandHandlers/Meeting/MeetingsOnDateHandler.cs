using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Exceptions;
using MeetingManagementApp.Domain.Models.Common;
using MeetingManagementApp.Domain.Models.Input;
using MeetingManagementApp.Infrastructure.AbstractHandlers;
using System.Globalization;
using System.Text.Json;

namespace MeetingManagementApp.Infrastructure.CommandHandlers.Meeting
{
    internal class MeetingsOnDateHandler : AbstractCommandHandler
    {
        private readonly IMeetingController _meetingController;

        public MeetingsOnDateHandler(IPrinterService consoleService, IMeetingController meetingController) : base(consoleService)
        {
            _meetingController = meetingController;
        }

        public override string? GetCommandDescription()
        {
            return "Показать встречи на конкретную дату.";
        }

        protected override CommandResult GetConsoleCommandResult(string? value)
        {
            Console.Clear();

            Console.WriteLine("Показать встречи на конкретную дату");
            Console.WriteLine(new string('-', 40));

            var meeting = string.IsNullOrEmpty(value) ? new MeetingInput() : JsonSerializer.Deserialize<MeetingInput>(value) ?? new MeetingInput();

            if (meeting.IsFirstCommandCall)
                meeting = new MeetingInput
                {
                    OnDate = meeting.OnDate,
                    IsFirstCommandCall = false
                };

            Console.WriteLine();

            var onDate = meeting.OnDate;

            if (onDate.HasValue)
            {
                Console.Write("Дата: ");

                Console.WriteLine($"{onDate.Value:dd.MM.yyyy}");
            }
            else
            {
                Console.Write("Введите дату: ");

                var onDateInput = Console.ReadLine()?.Split(".");

                onDate = onDateInput?.Length == 3 && DateTime.TryParse(string.Join("-", onDateInput[2], onDateInput[1], onDateInput[0]), CultureInfo.InvariantCulture, out var val) ? val.Date : throw new UserInputException("Введена неверная дата.", JsonSerializer.Serialize(meeting));
                meeting.OnDate = onDate;
            }

            var meetings = _meetingController.GetMeetingsOnDate(onDate.Value);


            if (meetings.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("Встречи отсутствуют.");
            }
            else
                foreach (var item in meetings)
                {
                    Console.WriteLine();
                    Console.WriteLine(new string('-', 40));

                    Console.WriteLine();
                    Console.WriteLine($"Номер встречи: {item.Id}");

                    Console.WriteLine();
                    Console.WriteLine($"Заголовок: {item.Subject}");

                    Console.WriteLine();
                    Console.WriteLine($"Начало: {item.MeetingStart:HH:mm}");

                    Console.WriteLine();
                    Console.WriteLine($"Примерное окончание: {item.MeetingEnd:HH:mm}");

                    Console.WriteLine();
                    Console.WriteLine($"Описание: {item.Description}");

                    Console.WriteLine();
                    Console.WriteLine($"Уведомление: {(item.MeetingNotification != null ? item.MeetingNotification.NotificationTime.ToString("dd.MM.yyyy HH:mm") : "Отсутствует")}");

                    Console.WriteLine();
                }

            return new CommandResult
            {
                ResultValue = meeting != null ? JsonSerializer.Serialize(new MeetingInput
                {
                    OnDate = onDate,
                    IsFirstCommandCall = true,
                }) : null,
            };
        }

        protected override IReadOnlyCollection<string> GetAllowedCommands(string? requestValue)
        {
            var meeting = string.IsNullOrEmpty(requestValue) ? new MeetingInput() : JsonSerializer.Deserialize<MeetingInput>(requestValue) ?? new MeetingInput();

            if (!meeting.OnDate.HasValue)
                throw new Exception("Ошибка при просмотре встреч.");

            var meetings = _meetingController.GetMeetingsOnDate(meeting.OnDate.Value);

            if (meetings.Count == 0)
                return meeting.OnDate >= DateTime.Today ? ["am", "d", "m", "q"] : ["d", "m", "q"];
            else
                return meeting.OnDate >= DateTime.Today ? ["am", "vm", "d", "m", "q"] : ["vm", "d", "m", "q"];
        }

        public override string GetCommand()
        {
            return "v";
        }
    }
}
