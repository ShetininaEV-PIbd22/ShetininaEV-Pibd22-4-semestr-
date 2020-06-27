using AbstractRemontBusinessLogic.Enums;
using System;
using System.Runtime.Serialization;

namespace AbstractRemontBusinessLogic.BindingModels
{
    /// Заказ     
    public class RemontBindingModel
    {
        public int? Id { get; set; }
        public int? ClientId { get; set; }
        public int ShipId { get; set; }
        public int? ImplementerId { get; set; }
        public int Count { get; set; }
        public decimal Sum { get; set; }
        public RemontStatus Status { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime? DateImplement { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool? FreeOrders { get; set; }
    }
}
