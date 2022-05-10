using Jobcity.Chat.ApplicationLayer.Contracts;
using Jobcity.Chat.ApplicationLayer.Implementations;
using Jobcity.Chat.Bot.Contracts;
using Jobcity.Chat.Bot.Implementations;
using Jobcity.Chat.InfraLayer.Constants;
using Jobcity.Chat.InfraLayer.Contracts;
using Jobcity.Chat.InfraLayer.Implementations;
using Jobcity.Chat.Mvc.SignalRChat;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Jobcity.Chat.Bot.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            ReceiveMessage(factory);
        }

        public static void ReceiveMessage(ConnectionFactory factory)
        {
            IChatApiProxy chatApiProxy = new ChatApiProxy();
            IChatBotRabbitProxy chatBotRabbitProxy = new ChatBotRabbitProxy();
            IChatMessageRepository chatMessageRepository = new ChatMessageRepository();
            IChatBotBL chatBotBL = new ChatBotBL(chatApiProxy, chatBotRabbitProxy);
            IChatMessageBL chatMessageBL = new ChatMessageBL(chatMessageRepository);

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "chatbot",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    ChatHub chatHub = new ChatHub(chatMessageBL, chatBotBL);
                    chatHub.SendMessage(BotMessages.Bot, message);
                };
                channel.BasicConsume(queue: "chatbot",
                                     autoAck: true,
                                     consumer: consumer);

            }
        }

    }
}
