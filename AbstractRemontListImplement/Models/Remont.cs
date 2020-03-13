using AbstractRemontBusinessLogic.Enums;
using System;

namespace AbstractRemontListImplement.Models
{
    /// <summary>
    /// Заказ
    /// </summary>
    public class Remont
    {
        public int Id { get; set; }
        public int ShipId { get; set; }
        public int Count { get; set; }
        public decimal Sum { get; set; }
        public RemontStatus Status { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime? DateImplement { get; set; }
    }
}
