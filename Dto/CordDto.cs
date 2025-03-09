using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Models;

namespace server.Dto
{
    public class CordDto
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
        public double AnsweredLat { get; set; }
        public double AnsweredLng { get; set; }
    }
}