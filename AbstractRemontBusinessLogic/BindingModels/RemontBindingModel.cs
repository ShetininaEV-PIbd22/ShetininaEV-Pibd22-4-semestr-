using AbstractRemontBusinessLogic.Enums;
using System;
using System.Runtime.Serialization;

namespace AbstractRemontBusinessLogic.BindingModels
{
    /// <summary>     
    /// Заказ     
    /// </summary>   
   // [DataContract]
    public class RemontBindingModel
    {
        public int? Id { get; set; }
        public int ShipId { get; set; }
        public int ClientId { set; get; }
        public string ClientFIO { set; get; }
        public int Count { get; set; }
        public decimal Sum { get; set; }
        public RemontStatus Status { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime? DateImplement { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        /*
        [DataMember]
        public int? Id { get; set; }
        [DataMember]
        public int? ClientId { get; set; }
        [DataMember]
        public int ShipId { get; set; }
        [DataMember]
        public int Count { get; set; }
        [DataMember]
        public decimal Sum { get; set; }
        [DataMember]
        public RemontStatus Status { get; set; }
        [DataMember]
        public DateTime DateCreate { get; set; }
        [DataMember]
        public DateTime? DateImplement { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        */
    }
}
