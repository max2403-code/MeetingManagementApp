using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Exceptions;
using MeetingManagementApp.Domain.Models.Common;
using MeetingManagementApp.Domain.Models.Input;
using MeetingManagementApp.Infrastructure.AbstractHandlers;
using System.Text.Json;

namespace MeetingManagementApp.Infrastructure.CommandHandlers
{
    internal class ViewMeetingHandler : AbstractCommandHandler
    {
        private readonly IMeetingController _meetingController;

        public ViewMeetingHandler(IReadOnlyDictionary<string, ICommandRequestHandler> nextHandlers, IPrinterService consoleService, IMeetingController meetingController) : base(nextHandlers, consoleService)
        {
            _meetingController = meetingController;
        }

        public override string? GetCommandDescription()
        {
            return "Просмотр встречи";
        }

        protected override CommandResult GetConsoleCommandResult(string? value)
        {
            Console.Clear();

            var meeting = string.IsNullOrEmpty(value) ? new MeetingInput() : JsonSerializer.Deserialize<MeetingInput>(value) ?? new MeetingInput();

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

                id = int.TryParse(idInput, out var val) ? val : throw new BusinessException("Введен некорректный номер встречи.");
            }

            var meetingDTO = _meetingController.GetMeetingById(id.Value, meeting.OnDate.Value);

            meeting.Id = id;
            meeting.Subject = meetingDTO.Subject;
            meeting.Description = meetingDTO.Description;
            meeting.OnDate = meetingDTO.MeetingStart.Date;
            meeting.MeetingStart = meetingDTO.MeetingStart;
            meeting.MeetingEnd = meetingDTO.MeetingEnd;
            meeting.MeetingNotification = meetingDTO.MeetingNotification != null ? new MeetingNotificationInput
            {
                MeetingId = meetingDTO.MeetingNotification.MeetingId,
                NotificationTime = meetingDTO.MeetingNotification.NotificationTime
            } : null;

            Console.WriteLine();
            Console.WriteLine(new string('-', 20));

            Console.WriteLine();
            Console.WriteLine($"Номер встречи: {meeting.Id}");

            Console.WriteLine();
            Console.WriteLine($"Заголовок: {meeting.Subject}");

            Console.WriteLine();
            Console.WriteLine($"Начало: {meeting.MeetingStart:HH:mm}");

            Console.WriteLine();
            Console.WriteLine($"Примерное окончание: {meeting.MeetingEnd:HH:mm}");

            Console.WriteLine();
            Console.WriteLine($"Описание: {meeting.Description}");

            Console.WriteLine();
            Console.WriteLine($"Уведомление: {(meeting.MeetingNotification != null ? meeting.MeetingNotification.NotificationTime.Value.ToString("dd.MM.yyyy HH:mm") : "Отсутствует")}");

            return new CommandResult
            {
                ResultValue = meeting != null ? JsonSerializer.Serialize(meeting) : null,
            };
        }

        protected override ISet<string>? GetNotAllowedCommands(string? requestValue)
        {
            var rval = new List<string>();

            var meeting = string.IsNullOrEmpty(requestValue) ? new MeetingInput() : JsonSerializer.Deserialize<MeetingInput>(requestValue) ?? new MeetingInput();

            if (!meeting.MeetingStart.HasValue)
                throw new Exception("Ошибка при просмотре встречи.");

            if (meeting.MeetingStart.Value < DateTime.Now)
                rval.AddRange(["um", "an", "vn"]);
            else
            {
                if (meeting.MeetingNotification != null)
                    rval.Add("an");
                else
                    rval.Add("vn");
            }

            return rval.ToHashSet();
        }
    }
}
