using Jobcity.Chat.InfraLayer.Constants;
using Jobcity.Chat.InfraLayer.Contracts;
using RabbitMQ.Client;
using System.Text;

namespace Jobcity.Chat.InfraLayer.Implementations
{
    public class ChatBotRabbitProxy : IChatBotRabbitProxy
    {
        public void SendMessage(string message)
        {
            var factory = new ConnectionFactory() { HostName = RabbitMQConfiguration.HostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: RabbitMQConfiguration.QueueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: "",
                                     routingKey: RabbitMQConfiguration.QueueName,
                                     basicProperties: properties,
                                     body: body);
            }

        }
    }
}
