using ImagX_API.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Entities
{
    public class Friendship : BaseEntity
    {
        public Friendship()
        {
            Created = DateTime.UtcNow;

        }
        public AppUser User1 { get; set; }
        public AppUser User2 { get; set; }
        public string User1ID { get; set; }
        public string User2ID { get; set; }
    }
}
