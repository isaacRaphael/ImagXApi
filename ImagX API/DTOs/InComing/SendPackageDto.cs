using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.DTOs.InComing
{
    public class SendPackageDto
    {
        public IFormFile Media { get; set; }
        public List<string> Usernames { get; set; }
        public string senderId { get; set; }
        public string Caption { get; set; }

    }
}
