using ImagX_API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImagX_API.Contracts
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<ICollection<Comment>> GetCommentsOfPost(int postId);
    }
    
}
