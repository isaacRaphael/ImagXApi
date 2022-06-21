using ImagX_API.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Entities
{
    public class Package : BaseEntity
    {
        public Package()
        {
            Created = DateTime.Now;
        }
        public string Image { get; set; }
        public string Caption { get; set; }
        public string senderId { get; set; }
        public string receiverId { get; set; }

    }
}
