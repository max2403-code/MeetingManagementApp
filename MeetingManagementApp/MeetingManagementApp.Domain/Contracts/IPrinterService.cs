using MeetingManagementApp.Domain.Models.Common;

namespace MeetingManagementApp.Domain.Contracts
{
    public interface IPrinterService
    {
        CommandResult PrinterExecute(
            string? value,
            Func<string?, CommandResult> func,
            IReadOnlyDictionary<string, ICommandRequestHandler>? handlers = null,
            Func<string?, IReadOnlyCollection<string>>? allowCommandFunc = null);
    }
}
