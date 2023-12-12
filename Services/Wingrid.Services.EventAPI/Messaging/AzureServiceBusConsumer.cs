using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Wingrid.Services.EventAPI.Models;
using Wingrid.Services.EventAPI.Services;

namespace Wingrid.Services.FixtureAPI.Messaging
{
    public interface IAzureServiceBusConsumer
    {
        Task Start();
        Task Stop();
    }

    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string eventFinalQueue;
        private readonly ILogger<AzureServiceBusConsumer> _logger;
        private readonly IConfiguration _configuration;
        private readonly ServiceBusProcessor _processor;
        public AzureServiceBusConsumer(IConfiguration configuration, ILogger<AzureServiceBusConsumer> logger)
        {
            _configuration = configuration;
            _logger = logger;
            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString") ?? throw new Exception("Missing ServiceBusConnectionString configuration.");
            eventFinalQueue = _configuration.GetValue<string>("TopicAndQueueNames:EventFinalQueue") ?? throw new Exception("Missing TopicAndQueueNames:EventFinalQueue configuration.");
            var client = new ServiceBusClient(serviceBusConnectionString);
            _processor = client.CreateProcessor(eventFinalQueue);
        }

        public async Task Start()
        {
            // _processor.ProcessMessageAsync += OnEventFinalReceived;
            _processor.ProcessErrorAsync += ErrorHandler;
            await _processor.StartProcessingAsync();
        }

        // private async Task OnEventFinalReceived(ProcessMessageEventArgs args)
        // {
        //     var message = args.Message;
        //     var body = Encoding.UTF8.GetString(message.Body);
        //     EventDto objMessage = JsonConvert.DeserializeObject<EventDto>(body) ?? throw new Exception("Error deserializing eventfinal body.");
        //     try
        //     {
        //         await _scoreService.ProcessFinalScore(objMessage);
        //         await args.CompleteMessageAsync(args.Message);
        //     }
        //     catch (Exception)
        //     {
        //         throw;
        //     }
        // }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            // _logger.LogError(args.Exception.ToString());
            return Task.CompletedTask;
        }

        public async Task Stop()
        {
            await _processor.StopProcessingAsync();
            await _processor.DisposeAsync();
        }
    }
}