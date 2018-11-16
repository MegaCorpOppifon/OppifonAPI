using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OppifonAPI.DTO
{
    public class DTOImage
    {
        public IFormFile Image { get; set; }
    }
}
