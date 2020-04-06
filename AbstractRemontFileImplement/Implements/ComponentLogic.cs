using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AbstractRemontBusinessLogic.BindingModels;
using AbstractRemontBusinessLogic.Interfaces;
using AbstractRemontBusinessLogic.ViewModels;
using AbstractRemontFileImplement.Models;

namespace AbstractRemontFileImplement.Implements
{
    public class ComponentLogic: IComponentLogic
    {
        private readonly FileDataListSingleton source;
        public ComponentLogic()
        {
            source = FileDataListSingleton.GetInstance();
        }
        public void CreateOrUpdate(ComponentBindingModel model)
        {
            Component element = source.Components.FirstOrDefault(rec => rec.ComponentName == model.ComponentName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть компонент с таким названием");
            }
            if (model.Id.HasValue)
            {
                element = source.Components.FirstOrDefault(rec => rec.Id == model.Id);
                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
            }
            else
            {
                int maxId = source.Components.Count > 0 ? source.Components.Max(rec => rec.Id) : 0;
                element = new Component { Id = maxId + 1 };
                source.Components.Add(element);
            }
            element.ComponentName = model.ComponentName;
        }
        public void Delete(ComponentBindingModel model)
        {
            Component element = source.Components.FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                source.Components.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
        public List<ComponentViewModel> Read(ComponentBindingModel model)
        {
            return source.Components
            .Where(rec => model == null || rec.Id == model.Id)
            .Select(rec => new ComponentViewModel
            {
                Id = rec.Id,
                ComponentName = rec.ComponentName
            })
            .ToList();
        }
        /*
        private readonly FileDataListSingleton source;
        public ComponentLogic()
        {
            source = FileDataListSingleton.GetInstance();
        }
        public void CreateOrUpdate(ComponentBindingModel model)
        {
            Component tempComponent = model.Id.HasValue ? null : new Component
            {
                Id = 1
            };
            foreach (var component in source.Components)
            {
                if (component.ComponentName == model.ComponentName && component.Id !=
               model.Id)
                {
                    throw new Exception("Уже есть заготовка с таким названием");
                }
                if (!model.Id.HasValue && component.Id >= tempComponent.Id)
                {
                    tempComponent.Id = component.Id + 1;
                }
                else if (model.Id.HasValue && component.Id == model.Id)
                {
                    tempComponent = component;
                }
            }
            if (model.Id.HasValue)
            {
                if (tempComponent == null)
                {
                    throw new Exception("Элемент не найден");
                }
                CreateModel(model, tempComponent);
            }
            else
            {
                source.Components.Add(CreateModel(model, tempComponent));
            }
        }
        public void Delete(ComponentBindingModel model)
        {
            for (int i = 0; i < source.Components.Count; ++i)
            {
                if (source.Components[i].Id == model.Id.Value)
                {
                    source.Components.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
        public List<ComponentViewModel> Read(ComponentBindingModel model)
        {
            List<ComponentViewModel> result = new List<ComponentViewModel>();
            foreach (var component in source.Components)
            {
                if (model != null)
                {
                    if (component.Id == model.Id)
                    {
                        result.Add(CreateViewModel(component));
                        break;
                    }
                    continue;
                }
                result.Add(CreateViewModel(component));
            }
            return result;
        }
        private Component CreateModel(ComponentBindingModel model, Component component)
        {
            component.ComponentName = model.ComponentName;
            return component;
        }
        private ComponentViewModel CreateViewModel(Component component)
        {
            return new ComponentViewModel
            {
                Id = component.Id,
                ComponentName = component.ComponentName
            };
        }
        */
    }
}
