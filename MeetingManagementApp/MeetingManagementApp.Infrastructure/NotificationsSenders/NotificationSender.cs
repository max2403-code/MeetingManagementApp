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
                var notifications = _notificationService.GetMeetingNotifications();

                if (notifications.Count == 0)
                    continue;

                foreach (var notification in notifications)
                    _notificationService.RemoveMeetingNotification(notification.MeetingId);

                var notificationsJson = JsonSerializer.Serialize(notifications);
                Task.Run(() => _printerService.PrinterExecute(notificationsJson, PrinterHandler));
            }
        }

        private CommandResult PrinterHandler(string? notificationsJson)
        {
            var notifications = JsonSerializer.Deserialize<IReadOnlyCollection<MeetingNotificationDTO>>(notificationsJson);

            foreach(var notification in notifications)
            {
                var meeting = _meetingController.GetMeetingById(notification.MeetingId);

                Console.WriteLine();

                Console.WriteLine(new string('-', 40));

                Console.WriteLine("Напоминание");

                Console.WriteLine($"{meeting.Subject}");

                Console.WriteLine($"Время начала встречи: {meeting.MeetingStart:dd.MM.yyyy HH:mm}");

                Console.WriteLine(new string('-', 40));

                Console.WriteLine();
            }

            Console.ReadKey();

            return new CommandResult();
        }
    }
}
