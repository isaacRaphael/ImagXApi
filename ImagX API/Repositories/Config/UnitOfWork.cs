using ImagX_API.Contracts;
using ImagX_API.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Repositories.Config
{
    public class UnitOfWork : IUnitOfWork
    {
        

        public UnitOfWork(AppDbContext context, 
            IUserRepository userRepository, 
            IBuddyRequestRepository buddyRequestRepository, 
            IPostRepository postRepository, 
            ILikeRepository likeRepository,
            ICommentRepository commentRepository)
        {
            
            Users = userRepository;
            Buddies = buddyRequestRepository;
            Posts = postRepository;
            Likes = likeRepository;
            Comments = commentRepository;
        }

        public IUserRepository Users { get; set; }
        public IBuddyRequestRepository Buddies { get; set; }
        public IPostRepository Posts { get; set; }
        public ILikeRepository Likes { get; set; }
        public ICommentRepository Comments { get ; set ; }
    }
}
