using ImagX_API.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Entities
{
    public class BuddyRequest : BaseEntity
    {
        public BuddyRequest()
        {
            Created = DateTime.UtcNow;
        }
        public string RecipientId { get; set; }
        public string SenderId { get; set; }
    }
}
