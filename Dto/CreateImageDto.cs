using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Dto
{
    public class CreateImageDto
    {
        public IFormFile Image { get; set; }
        public string TookBy { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
    }
}