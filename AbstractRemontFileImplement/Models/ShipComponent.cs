using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractRemontFileImplement.Models
{
    public class ShipComponent
    {
        public int Id { get; set; }
        public int ShipId { get; set; }
        public int ComponentId { get; set; }
        public int Count { get; set; }
    }
}
