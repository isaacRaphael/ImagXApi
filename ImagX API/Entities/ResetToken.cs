using ImagX_API.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Entities
{
    public class ResetToken : BaseEntity
    {
        public string EmailToken { get; set; }
        public string ResetPasswordToken { get; set; }

    }
}
