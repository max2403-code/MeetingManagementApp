using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Exceptions;
using MeetingManagementApp.Domain.Models.Common;
using MeetingManagementApp.Domain.Models.DTO;
using MeetingManagementApp.Domain.Models.Input;
using MeetingManagementApp.Infrastructure.AbstractHandlers;
using System.Text.Json;

namespace MeetingManagementApp.Infrastructure.CommandHandlers
{
    internal class AddNewMeetingHandler : AbstractCommandHandler
    {
        private readonly IMeetingController _meetingController;

        public AddNewMeetingHandler(IReadOnlyDictionary<string, ICommandRequestHandler> nextHandlers, IPrinterService consoleService, IMeetingController meetingController) : base(nextHandlers, consoleService)
        {
            _meetingController = meetingController;
        }

        public override string? GetCommandDescription()
        {
            return "Добавить встречу";
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

                var onDateInput = Console.ReadLine();
                onDate = DateTime.TryParse(onDateInput, out var val) ? val.Date : throw new BusinessException("Введена неверная дата");
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



            return new CommandResult
            {
                ResultValue = meeting != null ? JsonSerializer.Serialize(meeting) : null,
            };

        }

        protected override ISet<string>? GetNotAllowedCommands(string? requestValue)
        {
            return null;
        }
    }
}
