using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImagX_API.Services.HelperClasses
{
    public class EmailModel
    {
        public string Receipient { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
