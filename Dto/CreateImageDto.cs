using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Models;

namespace server.Dto
{
    public class CreateImageDto
    {
        public IFormFile Image { get; set; }
        public string TookBy { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public string? InstagramUrl { get; set; }
        public Sender? Sender { get; set; }
    }
}