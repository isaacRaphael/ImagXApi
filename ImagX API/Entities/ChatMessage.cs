using ImagX_API.Entities.Base;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImagX_API.Entities
{
    public class ChatMessage : BaseEntity
    {
        public int ChatId { get; set; }
        public string PayLoad { get; set; }
        public string MediaUri { get; set; }
    }
}
