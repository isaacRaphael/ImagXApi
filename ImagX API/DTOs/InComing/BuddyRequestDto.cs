using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.DTOs.InComing
{
    public class BuddyRequestDto
    {
        public string RecipientId { get; set; }
        public string SenderId { get; set; }
    }
}
