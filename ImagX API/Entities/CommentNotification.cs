using System;

namespace ImagX_API.Entities
{
    public class CommentNotification : Notification<Comment>
    {
        public CommentNotification()
        {
            Created = DateTime.UtcNow;
        }
    }
}
