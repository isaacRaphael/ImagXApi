using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.DTOs.OutGoing
{
    public class ReplyResponseDto
    {
        public int Id { get; set; }
        public string PayLoad { get; set; }
        public int CommentId { get; set; }
        public string ReplierId { get; set; }
    }
}
