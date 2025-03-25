using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Models.Common;
using MeetingManagementApp.Domain.Models.DTO;
using System.Text.Json;

namespace MeetingManagementApp.Infrastructure.NotificationsSenders
{
    internal class NotificationSender : IBackgroundService
    {
        private readonly IPrinterService _printerService;
        private readonly INotificationService _notificationService;
        private readonly IMeetingController _meetingController;

        public NotificationSender(IPrinterService printerService, INotificationService notificationService, IMeetingController meetingController)
        {
            _printerService = printerService;
            _notificationService = notificationService;
            _meetingController = meetingController;
        }

        public void Run()
        {
            while (true)
            {
                foreach (var notification in _notificationService.GetMeetingNotifications())
                {
                    var notificationJson = JsonSerializer.Serialize(notification);

                    Task.Run(() => _printerService.PrinterExecute(notificationJson, PrinterHandler));
                }
            }
        }

        private CommandResult PrinterHandler(string? notificationJson)
        {
            var notification = JsonSerializer.Deserialize<MeetingNotificationDTO>(notificationJson);
            var meeting = _meetingController.GetMeetingById(notification.MeetingId);

            Console.WriteLine();

            Console.WriteLine(new string('-', 20));

            Console.WriteLine("Напоминание");

            Console.WriteLine($"{meeting.Subject}");

            Console.WriteLine($"Время начала встречи: {meeting.MeetingStart:dd.MM.yyyy HH:mm}");

            Console.WriteLine(new string('-', 20));

            Console.WriteLine();

            return new CommandResult();
        }
    }
}
