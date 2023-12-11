

using System.Text;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace Wingrid.MessageBus
{
    public interface IMessageBus
    {
        Task PublishMessage(object message, string connectionString, string topic_queue_Name);
    }

    public class MessageBus : IMessageBus
    {
        public async Task PublishMessage(object message, string connectionString, string topic_queue_Name)
        {
            await using var client = new ServiceBusClient(connectionString);
            ServiceBusSender sender = client.CreateSender(topic_queue_Name);
            var jsonMessage = JsonConvert.SerializeObject(message);
            ServiceBusMessage finalMessage = new(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString(),
            };

            await sender.SendMessageAsync(finalMessage);
            await client.DisposeAsync();
        }

    }
}