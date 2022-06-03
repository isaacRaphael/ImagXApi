using System;

namespace ImagX_API.Entities
{
    public class RequestNotification : Notification<BuddyRequest>
    {
        public RequestNotification()
        {
            Created = DateTime.UtcNow;
        }
    }
}
