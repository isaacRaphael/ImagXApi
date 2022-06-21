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
    public class FriendshipRepository : GenericRepository<Friendship> , IFriendshipRepository
    {
        private readonly AppDbContext _context;

        public FriendshipRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ICollection<AppUser>> GetFriendsOfUser(string id)
        {
            var friendships = _context.Friendships.Where(x => x.User1ID == id || x.User2ID == id);
            var firstSet = friendships.Select(x => x.User1).Where(x => x.Id != id).ToList();
            var secondSet = friendships.Select(x => x.User2).Where(x => x.Id != id).ToList();

            var buddies = Enumerable.Concat(firstSet, secondSet);
            return await Task.Run(() => buddies.ToList());
        }

        public async Task<bool> RemoveFriendOfUser(string PartyAId, string PartyBId)
        {
            var friendShip = await _context.Friendships.Where(x => (x.User1ID == PartyAId && x.User2ID == PartyBId) || (x.User2ID == PartyBId && x.User1ID == PartyAId)).FirstOrDefaultAsync();
            _context.Friendships.Remove(friendShip);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
