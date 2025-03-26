using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Exceptions;
using MeetingManagementApp.Domain.Models.Common;
using MeetingManagementApp.Domain.Models.DTO;
using MeetingManagementApp.Domain.Models.Input;
using MeetingManagementApp.Infrastructure.AbstractHandlers;
using MeetingManagementApp.Infrastructure.Controllers;
using System.Text.Json;

namespace MeetingManagementApp.Infrastructure.CommandHandlers
{
    internal class UpdateMeetingHandler : AbstractCommandHandler
    {
        private readonly IMeetingController _meetingController;

        public UpdateMeetingHandler(IReadOnlyDictionary<string, ICommandRequestHandler> nextHandlers, IPrinterService consoleService, IMeetingController meetingController) : base(nextHandlers, consoleService)
        {
            _meetingController = meetingController;
        }

        public override string? GetCommandDescription()
        {
            return "Редактировать встречу.";
        }

        protected override ISet<string> GetAllowedCommands(string? requestValue)
        {
            return new HashSet<string>(["vm", "m", "q"]);
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

            var subject = meeting.Subject;

            if (subject != null)
            {
                Console.Write("Заголовок: ");

                Console.WriteLine($"{subject}");
            }
            else
            {
                Console.Write("Укажите заголовок: ");

                subject = Console.ReadLine();
                var error = _meetingController.ValidateMeetingSubject(subject);

                if (!string.IsNullOrEmpty(error))
                    throw new BusinessException(error, JsonSerializer.Serialize(meeting));

                meeting.Subject = subject;
            }

            var description = meeting.Description;

            if (description != null)
            {
                Console.Write("Описание: ");

                Console.WriteLine($"{description}");
            }
            else
            {
                Console.Write("Укажите описание: ");

                description = Console.ReadLine();
                var error = _meetingController.ValidateMeetingDescription(description);

                if (!string.IsNullOrEmpty(error))
                    throw new BusinessException(error, JsonSerializer.Serialize(meeting));

                meeting.Description = description;
            }

            var meetingStart = meeting.MeetingStart;

            if (meetingStart.HasValue)
            {
                Console.Write("Время начала встречи: ");

                Console.WriteLine($"{meetingStart.Value:HH:mm}");
            }
            else
            {
                Console.Write("Введите время начала встречи: ");

                var meetingStartInput = Console.ReadLine();
                meetingStart = DateTime.TryParse(string.Join(" ", onDate.Value.ToString("MM/dd/yyyy"), meetingStartInput), out var val) ? val.Date : throw new BusinessException("Введено неверное время начала встречи.");

                var error = _meetingController.ValidateMeetingMeetingStart(meetingStart.Value);

                if (!string.IsNullOrEmpty(error))
                    throw new BusinessException(error, JsonSerializer.Serialize(meeting));

                meeting.MeetingStart = meetingStart;
            }

            var meetingEnd = meeting.MeetingEnd;

            if (meetingEnd.HasValue)
            {
                Console.Write("Примерное время окончания встречи: ");

                Console.WriteLine($"{meetingEnd.Value:HH:mm}");
            }
            else
            {
                Console.Write("Введите примерное время окончания встречи: ");

                var meetingEndInput = Console.ReadLine();
                meetingEnd = DateTime.TryParse(string.Join(" ", onDate.Value.ToString("MM/dd/yyyy"), meetingEndInput), out var val) ? val.Date : throw new BusinessException("Введено неверное время начала встречи.");

                var error = _meetingController.ValidateMeetingMeetingEnd(meetingStart.Value, meetingEnd.Value);

                if (!string.IsNullOrEmpty(error))
                    throw new BusinessException(error, JsonSerializer.Serialize(meeting));

                meeting.MeetingEnd = meetingEnd;
            }

            meeting.Id = _meetingController.AddNewMeeting(new MeetingDTO
            {
                Subject = meeting.Subject,
                Description = meeting.Description,
                MeetingStart = meeting.MeetingStart.Value,
                MeetingEnd = meeting.MeetingEnd.Value
            });

            Console.WriteLine();

            Console.WriteLine("Встреча успешно добавлена.");

            return new CommandResult
            {
                ResultValue = meeting != null ? JsonSerializer.Serialize(meeting) : null,
            };
        }
    }
}
