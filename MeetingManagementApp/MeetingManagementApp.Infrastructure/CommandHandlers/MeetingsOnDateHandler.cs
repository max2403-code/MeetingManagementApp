using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Exceptions;
using MeetingManagementApp.Domain.Models.Common;
using MeetingManagementApp.Domain.Models.Input;
using MeetingManagementApp.Infrastructure.AbstractHandlers;
using System.Text.Json;

namespace MeetingManagementApp.Infrastructure.CommandHandlers
{
    internal class MeetingsOnDateHandler : AbstractCommandHandler
    {
        private readonly IMeetingController _meetingController;

        public MeetingsOnDateHandler(IEnumerable<ICommandRequestHandler> nextHandlers, IPrinterService consoleService, IMeetingController meetingController) : base(nextHandlers, consoleService)
        {
            _meetingController = meetingController;
        }

        public override string? GetCommandDescription()
        {
            return "Показать встречи на конкретную дату";
        }

        protected override CommandResult GetConsoleCommandResult(string? value)
        {
            Console.Clear();

            var meeting = string.IsNullOrEmpty(value) ? new MeetingInput() : JsonSerializer.Deserialize<MeetingInput>(value) ?? new MeetingInput();

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

                onDate = onDateInput?.Length == 3 && DateTime.TryParse(string.Join("/", onDateInput[1], onDateInput[0], onDateInput[2]), out var val) ? val.Date : throw new BusinessException("Введена неверная дата.");
                meeting.OnDate = onDate;
            }

            var meetings = _meetingController.GetMeetingsOnDate(onDate.Value);

            Console.WriteLine();

            if (meetings.Count == 0)
                Console.WriteLine("Встречи отсутствуют.");
            else
                foreach (var item in meetings)
                {
                    Console.WriteLine();
                    Console.WriteLine(new string('-', 20));

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
                }

            return new CommandResult
            {
                ResultValue = meeting != null ? JsonSerializer.Serialize(meeting) : null,
            };
        }

        protected override ISet<string> GetAllowedCommands(string? requestValue)
        {
            var meeting = string.IsNullOrEmpty(requestValue) ? new MeetingInput() : JsonSerializer.Deserialize<MeetingInput>(requestValue) ?? new MeetingInput();

            if (!meeting.OnDate.HasValue)
                throw new Exception("Ошибка при просмотре встреч.");

            var meetings = _meetingController.GetMeetingsOnDate(meeting.OnDate.Value);

            if (meetings.Count == 0)
                return new HashSet<string>(["am", "d", "m", "q"]);
            else
                return new HashSet<string>(["am", "vm", "d", "m", "q"]);
        }

        public override string GetCommand()
        {
            return "v";
        }
    }
}
