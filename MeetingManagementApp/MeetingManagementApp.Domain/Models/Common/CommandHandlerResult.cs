using MeetingManagementApp.Domain.Contracts;

namespace MeetingManagementApp.Domain.Models.Common
{
    public class CommandHandlerResult
    {
        /// <summary>
        /// JSON параметр команды.
        /// </summary>
        public string? Result { get; set; }

        /// <summary>
        /// Обработчик следующей команды.
        /// </summary>
        public ICommandRequestHandler? NextCommandRequestHandler { get; set; }
    }
}
