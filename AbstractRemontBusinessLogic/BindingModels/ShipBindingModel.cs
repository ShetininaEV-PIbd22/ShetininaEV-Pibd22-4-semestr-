using System.Collections.Generic;

namespace AbstractRemontBusinessLogic.BindingModels
{
    /// <summary>     
    /// Изделие, изготавливаемое в кондитерской     
    /// </summary> 
    public class ShipBindingModel
    {
        public int? Id { get; set; }

        public string ShipName { get; set; }

        public decimal Price { get; set; }

        public Dictionary<int, (string, int)> ShipComponents { get; set; }
    }
}
