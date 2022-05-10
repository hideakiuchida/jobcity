using Jobcity.Chat.Bot.Contracts;
using Jobcity.Chat.InfraLayer.Constants;
using Jobcity.Chat.InfraLayer.Contracts;
using System.Threading.Tasks;

namespace Jobcity.Chat.Bot.Implementations
{
    public class ChatBotBL : IChatBotBL
    {
        private readonly IChatApiProxy _chatApiProxy;
        private readonly IChatBotRabbitProxy _chatBotRabbitProxy;
        public ChatBotBL(IChatApiProxy chatApiProxy, IChatBotRabbitProxy chatBotRabbitProxy)
        {
            _chatApiProxy = chatApiProxy;
            _chatBotRabbitProxy = chatBotRabbitProxy;
        }

        public async Task<string> CallChatBotToGetStockCode(string message)
        {
            string stockCode = message.Split('=')[1] ?? string.Empty;
            string messageResponse = string.Empty;
            if (stockCode.Equals(Commands.AaplUsCode))
            {
                messageResponse = await _chatApiProxy.DownloadCsv(stockCode);
            }
            else
                messageResponse = BotMessages.StockCodeNotAvailable;

            return messageResponse;
        }

        public void SendMessageMQ(string message)
        {
            _chatBotRabbitProxy.SendMessage(message);
        }
    }
}
