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
    public class ReplyRepository : GenericRepository<Reply>,  IReplyRepository
    {
        private readonly AppDbContext _context;

        public ReplyRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ICollection<Reply>> GetCommentReplies(int commentId)
        {
            var replies = _context.Replies.Where(r => r.CommentId == commentId);
            if (replies is null)
                return null;

            return await replies.ToListAsync();
        }
    }
}
