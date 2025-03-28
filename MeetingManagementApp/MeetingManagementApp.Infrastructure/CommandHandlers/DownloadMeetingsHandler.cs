﻿using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Exceptions;
using MeetingManagementApp.Domain.Models.Common;
using MeetingManagementApp.Domain.Models.Input;
using MeetingManagementApp.Infrastructure.AbstractHandlers;
using System.Text.Json;

namespace MeetingManagementApp.Infrastructure.CommandHandlers
{
    internal class DownloadMeetingsHandler : AbstractCommandHandler
    {
        private readonly IMeetingController _meetingController;

        public DownloadMeetingsHandler(IPrinterService consoleService, IMeetingController meetingController) : base(consoleService)
        {
            _meetingController = meetingController;
        }

        public override string GetCommand()
        {
            return "d";
        }

        public override string? GetCommandDescription()
        {
            return "Скачать встречи за выбранную дату.";
        }

        protected override ISet<string> GetAllowedCommands(string? requestValue)
        {
            return new HashSet<string>(["v", "m", "q"]);
        }

        protected override CommandResult GetConsoleCommandResult(string? value)
        {
            Console.WriteLine();

            var meeting = string.IsNullOrEmpty(value) ? new MeetingInput() : JsonSerializer.Deserialize<MeetingInput>(value) ?? new MeetingInput();

            if (!meeting.OnDate.HasValue)
                throw new Exception("Ошибка скачивания.");

            Console.WriteLine("Укажите папку для скачивания:");

            var path = Console.ReadLine();

            if (string.IsNullOrEmpty(path) || !Path.IsPathRooted(path) || !Directory.Exists(path))
                throw new BusinessException("Указан неверный путь.");

            var t = _meetingController.SaveMeetingsOnDateFileAsync(meeting.OnDate.Value, path);

            t.Wait();

            Console.WriteLine("Файл успешно сохранен.");

            return new CommandResult();
        }
    }
}
