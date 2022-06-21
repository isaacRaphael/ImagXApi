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
            ICommentRepository commentRepository,
            IReplyRepository replyrepository,
            IFriendshipRepository friendshipRepository,
            INotificationRepository notificationRepository,
            IChatRepository chatRepository,
            ISharingKeyRepository sharingKeyRepository,
            IResetTokenRepository resetTokenRepository,
            IChatMessageRepository chatMessageRepository
            
            )
        {
            
            Users = userRepository;
            Buddies = buddyRequestRepository;
            Posts = postRepository;
            Likes = likeRepository;
            Comments = commentRepository;
            Replies = replyrepository;
            Friendships = friendshipRepository;
            Chats = chatRepository;
            Notifications = notificationRepository;
            Keys = sharingKeyRepository;
            ResetTokens = resetTokenRepository;
            Messages = chatMessageRepository;

        }

        public IUserRepository Users { get; set; }
        public IBuddyRequestRepository Buddies { get; set; }
        public IPostRepository Posts { get; set; }
        public ILikeRepository Likes { get; set; }
        public ICommentRepository Comments { get ; set ; }
        public IReplyRepository Replies { get  ; set ; }
        public IFriendshipRepository Friendships { get; set; }
        public INotificationRepository Notifications { get ; set ; }
        public IChatRepository Chats { get ; set ; }
        public ISharingKeyRepository Keys { get ; set ; }
        public IResetTokenRepository ResetTokens { get; set; }
        public IChatMessageRepository Messages { get ; set ; }
        public IPackageRepository Packages { get ; set ; }
        public IPackageImageRepository Images { get ; set ; }
    }
}
