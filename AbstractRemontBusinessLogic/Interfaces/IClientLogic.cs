using AbstractRemontBusinessLogic.BindingModels;
using AbstractRemontBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace AbstractRemontBusinessLogic.Interfaces
{
    public interface IClientLogic
    {
        List<ClientViewModel> Read(ClientBindingModel model);
        void CreateOrUpdate(ClientBindingModel model);
        void Delete(ClientBindingModel model);
    }
}
