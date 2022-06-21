using ImagX_API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Contracts
{
    public interface IFriendshipRepository : IGenericRepository<Friendship>
    {
        Task<ICollection<AppUser>> GetFriendsOfUser(string id);
        Task<bool> RemoveFriendOfUser(string PartyAId, string PartyBId);
    }
}
