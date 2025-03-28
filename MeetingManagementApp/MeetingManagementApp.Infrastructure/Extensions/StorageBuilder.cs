using MeetingManagementApp.Storage.Context;
using Microsoft.Extensions.DependencyInjection;

namespace MeetingManagementApp.Infrastructure.Extensions
{
    public static class StorageBuilder
    {
        public static void AddStorage(this IServiceCollection services)
        {
            services.AddSingleton<MeetingStorageContext>();
        }
    }
}
