using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string? Path { get; set; }
        public string? TookBy { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }

        public int? SenderId { get; set; }
        public Sender? Sender { get; set; } = new();
    }
}