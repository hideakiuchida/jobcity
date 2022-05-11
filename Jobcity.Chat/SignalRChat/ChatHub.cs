using Jobcity.Chat.ApplicationLayer.Contracts;
using Jobcity.Chat.Bot.Contracts;
using Jobcity.Chat.Domain;
using Jobcity.Chat.InfraLayer.Constants;
using Microsoft.AspNet.SignalR;
using System;
using System.Threading.Tasks;

namespace Jobcity.Chat.Mvc.SignalRChat
{
    public class ChatHub : Hub
    {
        private readonly IChatMessageBL _chatMessageBL;
        private readonly IChatBotBL _chatBotBL;
        public ChatHub(IChatMessageBL chatMessageBL, IChatBotBL chatBotBL)
        {
            _chatMessageBL = chatMessageBL;
            _chatBotBL = chatBotBL;
        }

        public async Task Send(string name, string message)
        {
            try
            {
                await SendMessageAsync(name, message);

                if (message.StartsWith(Commands.StockCode))
                    await _chatBotBL.CallChatBotToGetStockCode(message);
            }
            catch (Exception ex)
            {
                await SendMessageAsync(BotMessages.Bot, ex.Message);
            }
        }

        public async Task SendMessageAsync(string name, string message)
        {
            var chatMessage = new ChatMessage
            {
                UserName = name,
                DateTime = DateTime.Now.ToString(),
                Message = message
            };

            var chatRoomMessages = await _chatMessageBL.GetChatRoomMessagesAsync(chatMessage);

            Clients.All.refreshChatRoom(chatRoomMessages);
        }
    }
}