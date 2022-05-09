using Jobcity.Chat.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jobcity.Chat.ApplicationLayer.Contracts
{
    public interface IChatMessageBL
    {
        Task<List<ChatMessage>> GetChatRoomMessagesAsync(ChatMessage chatMessage);
    }
}
