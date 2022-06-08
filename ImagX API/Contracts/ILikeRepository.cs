using ImagX_API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImagX_API.Contracts
{
    public interface ILikeRepository : IGenericRepository<Like>
    {
        Task<ICollection<Like>> GetLikesOfUser(string userId);
        Task<ICollection<Like>> LikesOfAPost(int postId);
        Task<bool> UserHasLiked(string userId, int postId);
    }
    
}
