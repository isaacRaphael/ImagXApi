using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.DTOs.OutGoing
{
    public class LikeResponseDto
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string LikerId { get; set; }

    }
}
