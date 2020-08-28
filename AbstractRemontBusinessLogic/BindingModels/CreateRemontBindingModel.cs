using System.Runtime.Serialization;

namespace AbstractRemontBusinessLogic.BindingModels
{
    // Данные от клиента, для создания заказа
    [DataContract]
    public class CreateRemontBindingModel
    {
        [DataMember]
        public int ShipId { get; set; }
        [DataMember]
        public int ClientId { set; get; }
        [DataMember]
        public string ClientFIO { set; get; }
        [DataMember]
        public int Count { get; set; }
        [DataMember]
        public decimal Sum { get; set; }
    }
}
