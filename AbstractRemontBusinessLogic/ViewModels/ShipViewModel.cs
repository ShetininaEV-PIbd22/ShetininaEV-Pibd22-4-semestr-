using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace AbstractRemontBusinessLogic.ViewModels
{
    /// <summary>
    /// Изделие, изготавливаемое в кондитерской
    /// </summary>
    [DataContract]
    public class ShipViewModel
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        [DisplayName("Название корабля")]
        public string ShipName { get; set; }
        [DataMember]
        [DisplayName("Цена")]
        public decimal Price { get; set; }
        [DataMember]
        public Dictionary<int, (string, int)> ShipComponents { get; set; }
    }
}
