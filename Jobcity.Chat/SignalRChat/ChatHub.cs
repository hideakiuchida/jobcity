using Jobcity.Chat.ApplicationLayer.Contracts;
using Jobcity.Chat.Domain;
using Microsoft.AspNet.SignalR;
using System;

namespace Jobcity.Chat.Mvc.SignalRChat
{
    public class ChatHub : Hub
    {
        private readonly IChatMessageBL _chatMessageBL;
        public ChatHub(IChatMessageBL chatMessageBL)
        {
            _chatMessageBL = chatMessageBL;
        }

        public void Send(string name, string message)
        {
            ChatMessage chatMessage = new ChatMessage
            {
                UserName = name,
                DateTime = DateTime.Now.ToString(),
                Message = message
            };

            var chatRoomMessages =_chatMessageBL.GetChatRoomMessagesAsync(chatMessage).Result;

            Clients.All.refreshChatRoom(chatRoomMessages);
        }
    }
}