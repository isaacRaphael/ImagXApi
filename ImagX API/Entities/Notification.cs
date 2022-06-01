﻿using ImagX_API.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Entities
{
    public class Notification<T> : BaseEntity
    {
        public int RecipientID { get; set; }
        public int SenderID { get; set; }
        public T Action { get; set; }
    }

    
}