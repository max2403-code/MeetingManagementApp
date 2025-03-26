using MeetingManagementApp.Domain.Models.Common;

namespace MeetingManagementApp.Domain.Contracts
{
    public interface IPrinterService
    {
        CommandResult PrinterExecute(
            string? value, 
            Func<string?, CommandResult> func,
            IReadOnlyCollection<(string command, string? description)>? commands = null,
            Func<string?, ISet<string>>? allowCommandFunc = null);
    }
}
