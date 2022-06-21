using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.DTOs.InComing
{
    public class RemoveFriendDto
    {
        [Required]
        public string InitiatorId { get; set; }
        [Required]
        public string ReceiverId { get; set; }
    }
}
