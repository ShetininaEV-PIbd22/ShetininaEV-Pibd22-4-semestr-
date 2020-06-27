using AbstractRemontBusinessLogic.BindingModels;
using AbstractRemontBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbstractRemontBusinessLogic.Interfaces
{
    public interface ISkladLogic
    {
        List<SkladViewModel> Read(SkladBindingModel model);
        void CreateOrUpdate(SkladBindingModel model);
        void Delete(SkladBindingModel model);
        void AddComponent(SkladComponentBindingModel model);
    }
}
