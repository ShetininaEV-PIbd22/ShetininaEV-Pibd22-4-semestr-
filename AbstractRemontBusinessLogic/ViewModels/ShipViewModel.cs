using AbstractRemontBusinessLogic.Attributes;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace AbstractRemontBusinessLogic.ViewModels
{
    [DataContract]
    public class ShipViewModel : BaseViewModel
    {
        [Column(title: "Название корабля", gridViewAutoSize: GridViewAutoSize.Fill)]
        [DataMember]
        public string ShipName { get; set; }

        [Column(title: "Цена", width: 50)]
        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public Dictionary<int, (string, int)> ShipComponents { get; set; }

        public override List<string> Properties() => new List<string>
        {
            "Id",
            "ShipName",
            "Price"
        };
    }
}
