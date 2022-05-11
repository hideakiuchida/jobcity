using RabbitMQ.Client;
using System.Threading.Tasks;

namespace Jobcity.Chat.InfraLayer.Contracts
{
    public interface IChatBotRabbitProxy
    {
        void SendMessage(string message);
    }
}
