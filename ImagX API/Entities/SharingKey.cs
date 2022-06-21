using ImagX_API.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Entities
{
    public class SharingKey : BaseEntity
    {
        public SharingKey()
        {
            Key = Guid.NewGuid().ToString().Replace("-", "");
            Created = DateTime.UtcNow;
        }
        public string Key { get; set; }
        public string AppUserId { get; set; }
    }
}
