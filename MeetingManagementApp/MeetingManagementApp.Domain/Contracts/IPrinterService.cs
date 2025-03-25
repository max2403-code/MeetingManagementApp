using MeetingManagementApp.Domain.Models.Common;

namespace MeetingManagementApp.Domain.Contracts
{
    public interface IPrinterService
    {
        CommandResult? PrinterExecute(string? value, Func<string?, CommandResult?> func, Func<string?, IReadOnlyDictionary<string, string>>? allowCommandFunc = null);
    }
}
