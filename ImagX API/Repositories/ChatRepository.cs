using ImagX_API.Contracts;
using ImagX_API.Data;
using ImagX_API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Repositories
{
    public class ChatRepository : GenericRepository<Chat>, IChatRepository
    {
        private readonly AppDbContext _context;

        public ChatRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Chat> GetTwoPartChat(string partyAId, string partyBId)
        {
            var chat = await _context.Chats.Where(c => (c.PartyAId == partyAId && c.PartyBId == partyBId) || (c.PartyAId == partyBId && c.PartyBId == partyAId)).FirstOrDefaultAsync();
            if (chat is null)
                return null;

            return chat;
        }

        public async Task<bool> GetTwoPartChatExists(string partyAId, string partyBId)
        {
            var chat = await _context.Chats.Where(c => (c.PartyAId == partyAId && c.PartyBId == partyBId) || (c.PartyAId == partyBId && c.PartyBId == partyAId)).FirstOrDefaultAsync();
            if (chat is null)
                return false;
            return true;
        }

        public async Task<ICollection<Chat>> GetUserChats(string id)
        {
            var chats = await _context.Chats.Where(c => (c.PartyAId == id || c.PartyBId == id)).ToListAsync();
            if (chats is null)
                return null;
            return await Task.FromResult(chats);
        }
    }
}
