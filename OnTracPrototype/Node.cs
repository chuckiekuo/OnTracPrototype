using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data;

namespace OnTracPrototype
{
    public class Node
    {
        public string Id { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        // Probably deserialize...
        public string Address { get; set; }
    }
}
