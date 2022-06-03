using ImagX_API.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImagX_API.Entities
{
    public class Comment : BaseEntity
    {
        public Comment()
        {
            Created = DateTime.UtcNow;
        }
        public string PayLoad { get; set; }
        public DateTime Posted { get; set; }
        public Post Post { get; set; }
        public int PostId { get; set; }
        public AppUser AppUser { get; set; }
        [ForeignKey(nameof(AppUserId))]
        public int AppUserId { get; set; }
    }
}
