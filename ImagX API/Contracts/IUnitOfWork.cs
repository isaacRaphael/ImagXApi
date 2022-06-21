using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Contracts
{
    public interface IUnitOfWork
    {
        public IUserRepository Users { get; set; }
        public IBuddyRequestRepository Buddies { get; set; }
        public IPostRepository Posts { get; set; }
        public ILikeRepository Likes { get; set; }
        public ICommentRepository Comments { get; set; }
        public IReplyRepository Replies { get; set; }
        public IFriendshipRepository Friendships { get; set; }
        public INotificationRepository Notifications { get; set; }
        public IChatRepository Chats { get; set; }
        public ISharingKeyRepository Keys { get; set; }
        public IResetTokenRepository ResetTokens { get; set; }
        public IChatMessageRepository Messages { get; set; }
        public IPackageRepository Packages { get; set; }
        public IPackageImageRepository Images { get; set; }

    }
}
