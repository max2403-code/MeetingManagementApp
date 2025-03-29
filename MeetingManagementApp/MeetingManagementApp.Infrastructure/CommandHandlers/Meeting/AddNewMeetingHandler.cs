using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Exceptions;
using MeetingManagementApp.Domain.Models.Common;
using MeetingManagementApp.Domain.Models.DTO;
using MeetingManagementApp.Domain.Models.Input;
using MeetingManagementApp.Infrastructure.AbstractHandlers;
using System.Globalization;
using System.Text.Json;

namespace MeetingManagementApp.Infrastructure.CommandHandlers.Meeting
{
    internal class AddNewMeetingHandler : AbstractCommandHandler
    {
        private readonly IMeetingController _meetingController;

        public AddNewMeetingHandler(IPrinterService consoleService, IMeetingController meetingController) : base(consoleService)
        {
            _meetingController = meetingController;
        }

        public override string? GetCommandDescription()
        {
            return "Добавить встречу.";
        }

        protected override CommandResult GetConsoleCommandResult(string? value)
        {
            Console.Clear();

            Console.WriteLine("Добавить встречу");
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

                var error = _meetingController.ValidateMeetingMeetingOnDate(onDate.Value);

                if (!string.IsNullOrEmpty(error))
                    throw new UserInputException(error, JsonSerializer.Serialize(meeting));

                meeting.OnDate = onDate;
            }

            Console.WriteLine();

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
                    throw new UserInputException(error, JsonSerializer.Serialize(meeting));

                meeting.Subject = subject;
            }

            Console.WriteLine();

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
                    throw new UserInputException(error, JsonSerializer.Serialize(meeting));

                meeting.Description = description;
            }

            Console.WriteLine();

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

                meetingStart = DateTime.TryParse(string.Join(" ", onDate.Value.ToString("yyyy-MM-dd"), meetingStartInput + ":00"), CultureInfo.InvariantCulture, out var val) ? val : throw new UserInputException("Введено неверное время начала встречи.", JsonSerializer.Serialize(meeting));

                var error = _meetingController.ValidateMeetingMeetingStart(meetingStart.Value);


                if (!string.IsNullOrEmpty(error))
                    throw new UserInputException(error, JsonSerializer.Serialize(meeting));

                meeting.MeetingStart = meetingStart;
            }

            Console.WriteLine();

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
                meetingEnd = DateTime.TryParse(string.Join(" ", onDate.Value.ToString("yyyy-MM-dd"), meetingEndInput + ":00"), CultureInfo.InvariantCulture, out var val) ? val : throw new UserInputException("Введено неверное время окончания встречи.", JsonSerializer.Serialize(meeting));

                var error = _meetingController.ValidateMeetingMeetingEnd(meetingStart.Value, meetingEnd.Value);

                if (!string.IsNullOrEmpty(error))
                    throw new UserInputException(error, JsonSerializer.Serialize(meeting));

                meeting.MeetingEnd = meetingEnd;
            }

            meeting.Id = _meetingController.AddNewMeeting(new MeetingDTO
            {
                Subject = subject,
                Description = description,
                MeetingStart = meetingStart.Value,
                MeetingEnd = meetingEnd.Value
            });

            Console.WriteLine();

            Console.WriteLine("Встреча успешно добавлена.");

            return new CommandResult
            {
                ResultValue = meeting != null ? JsonSerializer.Serialize(new MeetingInput
                {
                    Id = meeting.Id,
                    IsFirstCommandCall = true
                }) : null,
            };
        }

        protected override IReadOnlyCollection<string> GetAllowedCommands(string? requestValue)
        {
            return ["vm", "m", "q"];
        }

        public override string GetCommand()
        {
            return "am";
        }
    }
}
