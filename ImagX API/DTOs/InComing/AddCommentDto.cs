using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.DTOs.InComing
{
    public class AddCommentDto
    {
        public string PayLoad { get; set; }
        public int PostId { get; set; }
        public string CommenterID { get; set; }
    }
}
