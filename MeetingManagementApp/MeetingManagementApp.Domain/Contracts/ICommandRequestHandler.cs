using MeetingManagementApp.Domain.Models.Common;

namespace MeetingManagementApp.Domain.Contracts
{
    public interface ICommandRequestHandler
    {
        /// <summary>
        /// Выполнение операций команды.
        /// </summary>
        /// <param name="requestValue"></param>
        /// <param name="handlers"></param>
        /// <returns></returns>
        CommandHandlerResult Execute(string? requestValue, IReadOnlyDictionary<string, ICommandRequestHandler> handlers);

        /// <summary>
        /// Получить описание команды.
        /// </summary>
        /// <returns></returns>
        string? GetCommandDescription() => null;

        /// <summary>
        /// Получить наименование вызова команды в консоли.
        /// </summary>
        /// <returns></returns>
        string GetCommand();

    }
}
