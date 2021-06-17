using Microsoft.Extensions.DependencyInjection;
using AnalyticsMicroservice.IRepository;
using AnalyticsMicroservice.Repository;
using AnalyticsMicroservice.Services;
namespace AnalyticsMicroservice.Services
{
    public static class ServicesCollectionExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            Hivemq mqtt = new Hivemq();
            services.AddSingleton(mqtt);
            services.AddScoped<IAnalyticsRepository, AnalyticsRepository>();
            services.AddSingleton(new DataService(mqtt));
            services.AddScoped<ISensorContext, SensorContext>();
        }
    }
}
