using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Infrastructure.CommandHandlers.Common;
using MeetingManagementApp.Infrastructure.CommandHandlers.Exceptions;
using MeetingManagementApp.Infrastructure.CommandHandlers.Meeting;
using MeetingManagementApp.Infrastructure.CommandHandlers.Notifications;
using MeetingManagementApp.Infrastructure.Controllers;
using MeetingManagementApp.Infrastructure.NotificationsSenders;
using MeetingManagementApp.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MeetingManagementApp.Infrastructure.Extensions
{
    public static class InfrastructureBuilder
    {
        public static void AddInfrastructure(this IServiceCollection services) 
        {
            services.AddSingleton<IPrinterService, ConsoleService>();
            services.AddSingleton<INotificationService, MeetingNotificationService>();
            services.AddSingleton<IBackgroundService, NotificationSender>();
            services.AddSingleton<IMeetingService, MeetingService>();
            services.AddSingleton<IMeetingController, MeetingController>();

            services.AddSingleton<ICommandRequestHandler, MainMenuHandler>();
            services.AddSingleton<ICommandRequestHandler, ExitHandler>();

            services.AddSingleton<ICommandRequestHandler, AddNewMeetingHandler>();
            services.AddSingleton<ICommandRequestHandler, UpdateMeetingHandler>();
            services.AddSingleton<ICommandRequestHandler, DeleteMeetingHandler>();
            services.AddSingleton<ICommandRequestHandler, ViewMeetingHandler>();
            services.AddSingleton<ICommandRequestHandler, MeetingsOnDateHandler>();
            services.AddSingleton<ICommandRequestHandler, DownloadMeetingsHandler>();

            services.AddSingleton<ICommandRequestHandler, AddNewMeetingNotificationHandler>();
            services.AddSingleton<ICommandRequestHandler, UpdateMeetingNotificationHandler>();
            services.AddSingleton<ICommandRequestHandler, DeleteMeetingNotificationHandler>();
            services.AddSingleton<ICommandRequestHandler, ViewMeetingNotificationHandler>();

            services.AddSingleton<ICommandRequestHandler, ExceptionHandler>();
            services.AddSingleton<ICommandRequestHandler, UserInputExceptionHandler>();
        }
    }
}
