using ImagX_API.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Entities
{
    public class Reply : BaseEntity
    {

        public string PayLoad { get; set; }
        public DateTime Posted { get; set; }
        public Comment Comment { get; set; }
        public int CommentId { get; set; }
        public AppUser AppUser { get; set; }
        [ForeignKey(nameof(AppUserId))]
        public int AppUserId { get; set; }
    }
}
