using System;

namespace ImagX_API.Entities
{
    public class LikeNotification : Notification<Like>
    {
        public LikeNotification()
        {
            Created = DateTime.UtcNow;

        }
    }
}
