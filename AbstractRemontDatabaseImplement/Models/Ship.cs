using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractRemontDatabaseImplement.Models
{
    public class Ship
    {
        public int Id { get; set; }
        [Required]
        public string ShipName { get; set; }
        [Required]
        public decimal Price { get; set; }
        public virtual ShipComponent ShipComponent { get; set; }

        public virtual List<Remont> Remonts { get; set; }
    }
}
