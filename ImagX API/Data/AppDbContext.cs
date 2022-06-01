using ImagX_API.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options ) : base(options)
        {

        }

        public DbSet<BuddyRequest> BuddyRequests { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<CommentNotification> CommentNotifications { get; set; }
        public DbSet<LikeNotification> LikeNotifications { get; set; }
        public DbSet<PostNotification> PostNotifications { get; set; }
        public DbSet<RequestNotification> RequestNotifications { get; set; }


    }
}
