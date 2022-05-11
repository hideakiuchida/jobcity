using Jobcity.Chat.Bot.Contracts;
using Jobcity.Chat.InfraLayer.Constants;
using Jobcity.Chat.InfraLayer.Contracts;
using System;
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
            string messageResponse;
            string stockCode = string.Empty;

            if (!string.IsNullOrEmpty(message))
            {
                stockCode = message.Split('=')[1] ?? stockCode;
            }

            if (stockCode.Equals(Commands.AaplUsCode))
            {
                messageResponse = await _chatApiProxy.DownloadCsv(stockCode);
                _chatBotRabbitProxy.SendMessage(messageResponse);
            }
            else
                messageResponse = BotMessages.StockCodeNotAvailable;

            return messageResponse;
        }
    }
}
