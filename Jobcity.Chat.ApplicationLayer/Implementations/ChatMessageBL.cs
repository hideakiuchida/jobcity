using Jobcity.Chat.ApplicationLayer.Contracts;
using Jobcity.Chat.Domain;
using Jobcity.Chat.InfraLayer.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jobcity.Chat.ApplicationLayer.Implementations
{
    public class ChatMessageBL : IChatMessageBL
    {
        private readonly IChatMessageRepository _chatMessageRepository;
        public ChatMessageBL(IChatMessageRepository chatMessageRepository)
        {
            _chatMessageRepository = chatMessageRepository;
        }

        public async Task<List<ChatMessage>> GetChatRoomMessagesAsync(ChatMessage chatMessage)
        {
            await _chatMessageRepository.AddChatMessagesAsync(chatMessage);
            var chatRoomMessages = await _chatMessageRepository.GetChatMessagesAsync();
            return chatRoomMessages;
        }
    }
}
