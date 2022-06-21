using ImagX_API.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Entities
{
    public class Chat : BaseEntity
    {
        public Chat()
        {
            Created = DateTime.UtcNow;
        }
        public AppUser PartyA { get; set; }
        public AppUser PartyB { get; set; }
        public string PartyAId { get; set; }
        public string PartyBId { get; set; }
        public List<ChatMessage> MyProperty { get; set; }
    }
}
