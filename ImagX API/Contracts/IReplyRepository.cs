using ImagX_API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImagX_API.Contracts
{
    public interface IReplyRepository : IGenericRepository<Reply>
    {
        Task<ICollection<Reply>> GetCommentReplies(int commentId);
    }
    
}
