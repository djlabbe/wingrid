

using System.Text;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace Wingrid.MessageBus
{
    public interface IMessageBus
    {
        Task PublishMessage(object message, string topic_queue_Name);
    }

    public class MessageBus : IMessageBus
    {
        //TODO: Move to AppSettings
        private readonly string connectionString = "Endpoint=sb://wingrid.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SPpLslOgEAW1E4Bf9jvbzC4RmBzqwc1/G+ASbG+De/0=";
        public async Task PublishMessage(object message, string topic_queue_Name)
        {
            await using var client = new ServiceBusClient(connectionString);
            ServiceBusSender sender = client.CreateSender(topic_queue_Name);
            var jsonMessage = JsonConvert.SerializeObject(message);
            ServiceBusMessage finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString(),
            };

            await sender.SendMessageAsync(finalMessage);
            await client.DisposeAsync();
        }

    }
}