using MeetingManagementApp.Domain.Contracts;
using MeetingManagementApp.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;


namespace MeetingManagementApp.Api
{
    public class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddStorage();
            services.AddInfrastructure();

            services.AddSingleton<MeetingApp.MeetingApp>();

            var sp = services.BuildServiceProvider();

            var app = sp.GetRequiredService<MeetingApp.MeetingApp>();

            app.Run();
        }
    }
}
