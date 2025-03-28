using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Infrastructure.CommandHandlers;
using MeetingManagementApp.Infrastructure.Controllers;
using MeetingManagementApp.Infrastructure.NotificationsSenders;
using MeetingManagementApp.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata.Ecma335;

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

            services.AddSingleton<ICommandRequestHandler, MeetingsOnDateHandler>();

            services.AddSingleton<ICommandRequestHandler, AddNewMeetingHandler>();

            services.AddSingleton<ICommandRequestHandler, DownloadMeetingsHandler>();

            services.AddSingleton<ICommandRequestHandler, ViewMeetingHandler>();
            services.AddSingleton<ICommandRequestHandler, UpdateMeetingHandler>();

            services.AddSingleton<ICommandRequestHandler, ExceptionHandler>();
            services.AddSingleton<ICommandRequestHandler, ExitHandler>();



            services.AddSingleton<ICommandRequestHandler, RemoveMeetingHandler>();



            //services.AddKeyedSingleton<ICommandRequestHandler, MainMenuHandler>("m");

        }
    }
}
