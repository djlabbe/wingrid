using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wingrid.Services.FixtureAPI.Messaging;

namespace Wingrid.Services.FixtureAPI.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        private static IAzureServiceBusConsumer? ServiceBusConsumer { get; set; }
        public static IApplicationBuilder UseAzureServiceBusConsumer(this IApplicationBuilder app)
        {
            ServiceBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>() ?? throw new Exception("Error loading IAzureServiceBusConsumer implementation.");
            var hostApplicationLife = app.ApplicationServices.GetService<IHostApplicationLifetime>();
            hostApplicationLife?.ApplicationStarted.Register(OnStart);
            hostApplicationLife?.ApplicationStopped.Register(OnStop);
            return app;
        }

        private static void OnStop()
        {
            ServiceBusConsumer?.Stop();
        }

        private static void OnStart()
        {
            ServiceBusConsumer?.Start();
        }
    }
}