using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractRemontFileImplement.Models
{
    public class SkladComponent
    {
        public int Id { get; set; }
        public int SkladId { get; set; }
        public int ComponentId { get; set; }
        public int Count { get; set; }
    }
}
