using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AbstractRemontRestApi.Models
{
    public class ShipModel
    {
        public int Id { get; set; }
        public string ShipName { get; set; }
        public decimal Price { get; set; }
    }
}
