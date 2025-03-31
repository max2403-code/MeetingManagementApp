using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Domain.Models.Common;
using MeetingManagementApp.Domain.Models.DTO;
using System.Text.Json;

namespace MeetingManagementApp.Infrastructure.NotificationsSenders
{
    // TODO: Лучше перенести в отдельное приложение.
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
                try
                {
                    var notifications = _notificationService.GetMeetingNotificationsWithTimeRange();

                    if (notifications.Count == 0)
                        continue;

                    foreach (var notification in notifications)
                        _notificationService.RemoveMeetingNotification(notification.MeetingId);

                    var notificationsJson = JsonSerializer.Serialize(notifications);
                    Task.Run(() => _printerService.PrinterExecute(notificationsJson, PrinterHandler));
                }
                catch
                {
                    _printerService.PrinterExecute(null, ExcPrinterHandler);
                    break;
                }
            }
        }

        /// <summary>
        /// Обработчик на случай падения сендера.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private CommandResult ExcPrinterHandler(string? value)
        {
            Console.WriteLine();

            Console.WriteLine("Отправка напоминаний экстренно прекращена вследствие ошибки!");

            Console.ReadKey();

            return new CommandResult();
        }

        /// <summary>
        /// Инструкции для обработчика уведомления.
        /// </summary>
        /// <param name="notificationsJson"></param>
        /// <returns></returns>
        private CommandResult PrinterHandler(string? notificationsJson)
        {
            try
            {
                var notifications = JsonSerializer.Deserialize<IReadOnlyCollection<MeetingNotificationDTO>>(notificationsJson);

                foreach (var notification in notifications)
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
            }
            catch
            {
                Console.WriteLine();
                Console.WriteLine("Ошибка при отправке уведомлений!");
            }
            finally
            {
                Console.ReadKey();
            }

            return new CommandResult();
        }
    }
}
