using System.Threading.Tasks;

namespace Jobcity.Chat.Bot.Contracts
{
    public interface IChatBotBL
    {
        Task<string> CallChatBotToGetStockCode(string stockCode);
        void SendMessageMQ(string message);
    }
}
