using Mangoes.Services.EmailAPI.Messaging;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata;

namespace Mangoes.Services.EmailAPI.Extension
{
    public static class ApplicationBuilderExtension
    {
        private static IAzureBusServiceConsumer AzureBusServiceConsumer { get; set; }
        public static IApplicationBuilder UserAzureBusServiceConsumer(this IApplicationBuilder app)
        {
            AzureBusServiceConsumer = app.ApplicationServices.GetService<IAzureBusServiceConsumer>();
            var hostApplicationLife = app.ApplicationServices.GetService<IHostApplicationLifetime>();
            hostApplicationLife.ApplicationStarted.Register(OnStart);
            hostApplicationLife.ApplicationStopping.Register(OnStop);
            return app;
        }

        private static void OnStart()
        {
            AzureBusServiceConsumer.Start();
        }
        private static void OnStop()
        {
            AzureBusServiceConsumer.Stop();
        }
    }
}
