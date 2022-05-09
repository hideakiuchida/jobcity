using Jobcity.Chat.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jobcity.Chat.InfraLayer.Contracts
{
    public interface IChatMessageRepository
    {
        Task<List<ChatMessage>> GetChatMessagesAsync();
        Task AddChatMessagesAsync(ChatMessage chatMessage);
    }
}
