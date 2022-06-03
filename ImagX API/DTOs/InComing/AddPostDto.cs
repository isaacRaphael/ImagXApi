using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.DTOs.InComing
{
    public class AddPostDto
    {
        public string  Caption { get; set; }
        public string UserId { get; set; }
        public IFormFile Media { get; set; }

    }
}
