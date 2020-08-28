using AbstractRemontBusinessLogic.BindingModels;
using AbstractRemontBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace AbstractRemontBusinessLogic.Interfaces
{
    public interface IRemontLogic
    {
        List<RemontViewModel> Read(RemontBindingModel model);
        void CreateOrUpdate(RemontBindingModel model);
        void Delete(RemontBindingModel model);
    }
}
