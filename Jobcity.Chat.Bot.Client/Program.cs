using Jobcity.Chat.ApplicationLayer.Contracts;
using Jobcity.Chat.ApplicationLayer.Implementations;
using Jobcity.Chat.Bot.Contracts;
using Jobcity.Chat.Bot.Implementations;
using Jobcity.Chat.Domain;
using Jobcity.Chat.InfraLayer.Constants;
using Jobcity.Chat.InfraLayer.Contracts;
using Jobcity.Chat.InfraLayer.Implementations;
using Jobcity.Chat.Mvc.SignalRChat;
using Microsoft.AspNet.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jobcity.Chat.Bot.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = RabbitMQConfiguration.HostName };
            ReceiveMessage(factory);
        }

        public static void ReceiveMessage(ConnectionFactory factory)
        {
            IChatApiProxy chatApiProxy = new ChatApiProxy();
            IChatBotRabbitProxy chatBotRabbitProxy = new ChatBotRabbitProxy();
            IChatMessageRepository chatMessageRepository = new ChatMessageRepository();
            IChatBotBL chatBotBL = new ChatBotBL(chatApiProxy, chatBotRabbitProxy);
            IChatMessageBL chatMessageBL = new ChatMessageBL(chatMessageRepository);

            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: RabbitMQConfiguration.QueueName,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                Console.WriteLine(" [*] Waiting for messages.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (sender, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);

                    var chatMessage = new ChatMessage
                    {
                        UserName = BotMessages.Bot,
                        DateTime = DateTime.Now.ToString(),
                        Message = message
                    };

                    var chatRoomMessages = chatMessageBL.GetChatRoomMessagesAsync(chatMessage).Result;

                    context.Clients.All.refreshChatRoom(chatRoomMessages);

                    Console.WriteLine(" [x] Done");

                    // Note: it is possible to access the channel via
                    //       ((EventingBasicConsumer)sender).Model here
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                };
                channel.BasicConsume(queue: RabbitMQConfiguration.QueueName,
                                     autoAck: false,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

    }
}
