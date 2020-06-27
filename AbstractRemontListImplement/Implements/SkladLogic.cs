using AbstractRemontBusinessLogic.BindingModels;
using AbstractRemontBusinessLogic.Interfaces;
using AbstractRemontBusinessLogic.ViewModels;
using AbstractRemontListImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbstractRemontListImplement.Implements
{
    public class SkladLogic: ISkladLogic
    {
        private readonly DataListSingleton source;

        public SkladLogic()
        {
            source = DataListSingleton.GetInstance();
        }

        public void CreateOrUpdate(SkladBindingModel model)
        {
            Sklad tempSklad = model.Id.HasValue ? null : new Sklad
            {
                Id = 1
            };
            foreach (var s in source.Sklads)
            {
                if (s.SkladName == model.SkladName && s.Id != model.Id)
                {
                    throw new Exception("Уже есть склад с таким названием");
                }
                if (!model.Id.HasValue && s.Id >= tempSklad.Id)
                {
                    tempSklad.Id = s.Id + 1;
                }
                else if (model.Id.HasValue && s.Id == model.Id)
                {
                    tempSklad = s;
                }
            }
            if (model.Id.HasValue)
            {
                if (tempSklad == null)
                {
                    throw new Exception("Элемент не найден");
                }
                CreateModel(model, tempSklad);
            }
            else
                source.Sklads.Add(CreateModel(model, tempSklad));
        }
    
        public void Delete(SkladBindingModel model)
        {
            for (int i = 0; i < source.SkladComponents.Count; ++i)
            {
                if (source.SkladComponents[i].SkladId == model.Id)
                {
                    source.SkladComponents.RemoveAt(i--);
                }
            }
            for (int i = 0; i < source.Sklads.Count; ++i)
                if (source.Sklads[i].Id == model.Id)
                {
                    source.Sklads.RemoveAt(i);
                    return;
                }
            throw new Exception("Элемент не найден");
        }

        public List<SkladViewModel> Read(SkladBindingModel model)
        {
            List<SkladViewModel> result = new List<SkladViewModel>();
            foreach (var storage in source.Sklads)
            {
                if (model != null)
                {
                    if (storage.Id == model.Id)
                    {
                        result.Add(CreateViewModel(storage));
                        break;
                    }
                    continue;
                }
                result.Add(CreateViewModel(storage));
            }
            return result;
        }

        private Sklad CreateModel(SkladBindingModel model, Sklad sklad)
        {
            sklad.SkladName = model.SkladName;
            return sklad;
        }

        private SkladViewModel CreateViewModel(Sklad storage)
        {
            // требуется дополнительно получить список компонентов для хранилища с
            // названиями и их количество
            Dictionary<int, (string, int)> storageComponents = new Dictionary<int, (string, int)>();
            foreach (var sm in source.SkladComponents)
            {
                if (sm.SkladId == storage.Id)
                {
                    string componentName = string.Empty;
                    foreach (var component in source.Components)
                    {
                        if (sm.ComponentId == component.Id)
                        {
                            componentName = component.ComponentName;
                            break;
                        }
                    }
                    storageComponents.Add(sm.ComponentId, (componentName, sm.Count));
                }
            }
            return new SkladViewModel
            {
                Id = storage.Id,
                SkladName = storage.SkladName,
                SkladComponents = storageComponents
            };
        }
        
        public void AddComponent(SkladComponentBindingModel model)
        {
            for (int i = 0; i < source.SkladComponents.Count; ++i)
            {
                if (source.SkladComponents[i].SkladId == model.SkladId &&
                    source.SkladComponents[i].ComponentId == model.ComponentId)
                {
                    source.SkladComponents[i].Count += model.Count;
                    model.Id = source.SkladComponents[i].Id;
                    return;
                }
            }
            int maxSCId = 0;
            for (int i = 0; i < source.SkladComponents.Count; ++i)
            {
                if (source.SkladComponents[i].Id > maxSCId)
                {
                    maxSCId = source.SkladComponents[i].Id;
                }
            }

            if (model.Id == 0)
            {
                source.SkladComponents.Add(new SkladComponent
                {
                    Id = ++maxSCId,
                    SkladId = model.SkladId,
                    ComponentId = model.ComponentId,
                    Count = model.Count
                });
            }
        }
    }
}
