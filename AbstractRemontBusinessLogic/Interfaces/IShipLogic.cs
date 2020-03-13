using AbstractRemontBusinessLogic.BindingModels;
using AbstractRemontBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace AbstractRemontBusinessLogic.Interfaces
{
    public interface IShipLogic
    {
        List<ShipViewModel> Read(ShipBindingModel model);
        void CreateOrUpdate(ShipBindingModel model);
        void Delete(ShipBindingModel model);
    }
}
