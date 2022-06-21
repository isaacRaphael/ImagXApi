using ImagX_API.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Entities
{
    public class Post : BaseEntity
    {
        public Post()
        {
            Created = DateTime.UtcNow;

        }
        public string Caption { get; set; }
        public string ImageUri { get; set; }
        public AppUser AppUser { get; set; }
        [ForeignKey(nameof(AppUserId))]
        public string AppUserId { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }

    }
}
