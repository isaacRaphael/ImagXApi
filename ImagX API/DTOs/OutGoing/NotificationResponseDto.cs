using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.DTOs.OutGoing
{
    public class NotificationResponseDto
    {
        public int Id { get; set; }
        public string RecipientID { get; set; }
        public string SenderID { get; set; }
        public string ActionType { get; set; }
        public int ActionId { get; set; }
        public string Message { get; set; }
    }
}
