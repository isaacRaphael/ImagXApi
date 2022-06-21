using ImagX_API.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Entities
{
    public class Notification : BaseEntity
    {
        public Notification()
        {
            Created = DateTime.UtcNow;
        }
        public string RecipientID { get; set; }
        public string SenderID { get; set; }
        public string ActionType { get; set; }
        public int ActionId { get; set; }
        public string Message { get; set; }

    }

    
}
