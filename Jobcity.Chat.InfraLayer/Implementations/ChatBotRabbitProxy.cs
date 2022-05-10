using Jobcity.Chat.InfraLayer.Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Jobcity.Chat.InfraLayer.Implementations
{
    public class ChatBotRabbitProxy : IChatBotRabbitProxy
    {
        private readonly ConnectionFactory factory;
        public ChatBotRabbitProxy()
        {
            factory = new ConnectionFactory() { HostName = "localhost" };
        }

        public void SendMessage(string message)
        {
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "chatbot",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);


                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: "chatbot",
                                     basicProperties: null,
                                     body: body);
            }

        }
    }
}
