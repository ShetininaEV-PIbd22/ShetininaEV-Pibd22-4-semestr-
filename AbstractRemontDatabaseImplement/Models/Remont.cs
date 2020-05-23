using AbstractRemontBusinessLogic.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractRemontDatabaseImplement.Models
{
    public class Remont
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int ShipId { get; set; }
        [Required]
        public string ClientFIO { set; get; }
        [Required]
        public int Count { get; set; }
        [Required]
        public decimal Sum { get; set; }
        [Required]
        public RemontStatus Status { get; set; }
        [Required]
        public DateTime DateCreate { get; set; }
        public DateTime? DateImplement { get; set; }
        public Ship Ship { get; set; }
        public Client Client { get; set; }
    }
}
