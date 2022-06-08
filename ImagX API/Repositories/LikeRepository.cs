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
    public class LikeRepository : GenericRepository<Like>, ILikeRepository
    {
        private readonly AppDbContext _context;

        public LikeRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ICollection<Like>> GetLikesOfUser(string userId)
        {
            return await _context.Likes.Where(l => l.AppUserId == userId).ToListAsync();
        }

        public async Task<ICollection<Like>> LikesOfAPost(int postId)
        {
            return await _context.Likes.Where(l => l.PostId == postId).ToListAsync();
        }

        public async Task<bool> UserHasLiked(string userId, int postId)
        {
            var postlikes = await LikesOfAPost(postId);
            if (postlikes is null)
                return false;

            return postlikes.Any(x => x.AppUserId == userId);
        }
    }
}
