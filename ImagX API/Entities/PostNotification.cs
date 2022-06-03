using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Entities
{
    public class PostNotification : Notification<Post>
    {
        public PostNotification()
        {
            Created = DateTime.UtcNow;

        }
    }
}
