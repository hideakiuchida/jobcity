using Jobcity.Chat.ApplicationLayer.Contracts;
using Jobcity.Chat.Bot.Contracts;
using Jobcity.Chat.Domain;
using Jobcity.Chat.InfraLayer.Constants;
using Microsoft.AspNet.SignalR;
using System;

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

        public void Send(string name, string message)
        {
            SendMessage(name, message);

            if (message.StartsWith(Commands.StockCode))
                ProcessStockCode(message);
        }

        public void SendMessage(string name, string message)
        {
            var chatMessage = GetNewChatMessage(name, message);

            var chatRoomMessages = _chatMessageBL.GetChatRoomMessagesAsync(chatMessage).Result;

            Clients.All.refreshChatRoom(chatRoomMessages);
        }

        private void ProcessStockCode(string message)
        {        
            string messageResponse = _chatBotBL.CallChatBotToGetStockCode(message).Result;
            var chatMessage = GetNewChatMessage(BotMessages.Bot, messageResponse);
            var chatRoomMessages = _chatMessageBL.GetChatRoomMessagesAsync(chatMessage).Result;
            Clients.All.refreshChatRoom(chatRoomMessages);
        }

        private ChatMessage GetNewChatMessage(string name, string message)
        {
            ChatMessage chatMessage = new ChatMessage
            {
                UserName = name,
                DateTime = DateTime.Now.ToString(),
                Message = message
            };
            return chatMessage;
        }
    }
}