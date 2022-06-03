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
    public class PostRepository : GenericRepository<Post> , IPostRepository
    {
        private readonly AppDbContext _context;

        public PostRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async  Task<ICollection<Post>> GetPostByUser(string id)
        {
            var posts = _context.Posts.Where(x => x.AppUserId == id);
            if (posts is null)
                return null;

            return await posts.ToListAsync();
        }

        public async Task<ICollection<Post>> GetPostsOfBuddies(string id)
        {
            var friendships = _context.Friendships.Where(x => x.User1ID == id || x.User2ID == id);
            var firstSet = friendships.Select(x => x.User1).Where(x => x.Id != id).ToList();
            var secondSet = friendships.Select(x => x.User2).Where(x => x.Id != id).ToList();

            var buddies = Enumerable.Concat(firstSet, secondSet).ToList();

            var res = new List<Post>();
            foreach(var bud in buddies)
            {
                foreach (var item in _context.Posts)
                {
                    if (item.AppUserId == bud.Id)
                        res.Add(item);
                }
            }

            return await Task.FromResult(res);
        }
    }
}
