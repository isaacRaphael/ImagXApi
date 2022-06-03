using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.DTOs.OutGoing
{
    public class PostResponseDto
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Caption { get; set; }
        public string ImageUrl { get; set; }
    }
}
