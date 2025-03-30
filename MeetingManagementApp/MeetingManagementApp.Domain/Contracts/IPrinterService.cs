using MeetingManagementApp.Domain.Models.Common;

namespace MeetingManagementApp.Domain.Contracts
{
    public interface IPrinterService
    {
        /// <summary>
        /// Отрисовка и взаимодействие с пользователем (в данном случае в консоли). 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="func"></param>
        /// <param name="handlers"></param>
        /// <param name="allowCommandFunc"></param>
        /// <returns></returns>
        CommandResult PrinterExecute(
            string? value,
            Func<string?, CommandResult> func,
            IReadOnlyDictionary<string, ICommandRequestHandler>? handlers = null,
            Func<string?, IReadOnlyCollection<string>>? allowCommandFunc = null);
    }
}
