using ImagX_API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImagX_API.Contracts
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task<ICollection<Post>> GetPostByUser(string id);
        Task<ICollection<Post>> GetPostsOfBuddies(string id);
    }
    
}
