using Jobcity.Chat.Domain;
using Jobcity.Chat.InfraLayer.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jobcity.Chat.InfraLayer.Implementations
{
    public class ChatMessageRepository : IChatMessageRepository
    {
        private static List<ChatMessage> chatMessages = new List<ChatMessage>();

        public async Task AddChatMessagesAsync(ChatMessage chatMessage)
        {
            chatMessages.Add(chatMessage);
        }

        public async Task<List<ChatMessage>> GetChatMessagesAsync()
        {
            var last50Messages = chatMessages.OrderByDescending(x => x.DateTime).Take(50).ToList();
            last50Messages = last50Messages.OrderBy(x => x.DateTime).ToList();
            return last50Messages;
        }
    }
}
