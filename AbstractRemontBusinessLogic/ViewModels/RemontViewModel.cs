using AbstractRemontBusinessLogic.Enums;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace AbstractRemontBusinessLogic.ViewModels
{
    /// <summary>
    /// Заказ
    /// </summary>
    [DataContract]
    public class RemontViewModel
    {
        [DataMember]
        public int Id { get; set; }
        
        [DataMember]
        public int ClientId { get; set; }
        [DataMember]
        public int ShipId { get; set; }
        
        [DataMember]
        [DisplayName("Клиент")]
        public string ClientFIO { get; set; }
        
        [DataMember]
        [DisplayName("Корабль")]
        public string ShipName { get; set; }
        
        [DataMember]
        [DisplayName("Количество")]
        public int Count { get; set; }
        
        [DataMember]
        [DisplayName("Сумма")]
        public decimal Sum { get; set; }
        
        [DataMember]
        [DisplayName("Статус")]
        public RemontStatus Status { get; set; }
        
        [DataMember]
        [DisplayName("Дата создания")]
        public DateTime DateCreate { get; set; }
        
        [DataMember]
        [DisplayName("Дата выполнения")]
        public DateTime? DateImplement { get; set; }
    }
}
