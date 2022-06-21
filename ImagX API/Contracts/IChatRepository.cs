using ImagX_API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Contracts
{
    public interface IChatRepository : IGenericRepository<Chat>
    {
        Task<ICollection<Chat>> GetUserChats(string id);

        Task<Chat> GetTwoPartChat(string partyAId, string partyBId);
        Task<bool> GetTwoPartChatExists(string partyAId, string partyBId);
    }
}
