using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Dto
{
    public class UpdateImageDto
    {
        public int ImageId { get; set; }
        public bool Drafted { get; set; }
    }
}