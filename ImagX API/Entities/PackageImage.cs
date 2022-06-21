using ImagX_API.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Entities
{
    public class PackageImage : BaseEntity
    {
        public PackageImage()
        {
            Created = DateTime.Now;
        }

        public string ImageUrl { get; set; }
        public int PackageId { get; set; }
        public Package Package { get; set; }

    }
}
