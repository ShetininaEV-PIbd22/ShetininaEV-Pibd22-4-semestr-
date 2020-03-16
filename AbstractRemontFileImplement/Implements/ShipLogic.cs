using AbstractRemontBusinessLogic.BindingModels;
using AbstractRemontBusinessLogic.Interfaces;
using AbstractRemontBusinessLogic.ViewModels;
using AbstractRemontFileImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractRemontFileImplement.Implements
{
    public class ShipLogic : IShipLogic
    {
        private readonly FileDataListSingleton source;
        public ShipLogic()
        {
            source = FileDataListSingleton.GetInstance();
        }
        public void CreateOrUpdate(ShipBindingModel model)
        {
            Ship element = source.Ships.FirstOrDefault(rec => rec.ShipName == model.ShipName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть корабль с таким названием");
            }
            if (model.Id.HasValue)
            {
                element = source.Ships.FirstOrDefault(rec => rec.Id == model.Id);
                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
            }
            else
            {
                int maxId = source.Ships.Count > 0 ? source.Components.Max(rec => rec.Id) : 0;
                element = new Ship { Id = maxId + 1 };
                source.Ships.Add(element);
            }
            element.ShipName = model.ShipName;
            element.Price = model.Price;
            // удалили те, которых нет в модели
            source.ShipComponents.RemoveAll(rec => rec.ShipId == model.Id && !model.ShipComponents.ContainsKey(rec.ComponentId));
            // обновили количество у существующих записей
            var updateComponents = source.ShipComponents.Where(rec => rec.ShipId == model.Id && model.ShipComponents.ContainsKey(rec.ComponentId));
            foreach (var updateComponent in updateComponents)
            {
                updateComponent.Count = model.ShipComponents[updateComponent.ComponentId].Item2;
                model.ShipComponents.Remove(updateComponent.ComponentId);
            }
            // добавили новые
            int maxPCId = source.ShipComponents.Count > 0 ? source.ShipComponents.Max(rec => rec.Id) : 0;
            foreach (var pc in model.ShipComponents)
            {
                source.ShipComponents.Add(new ShipComponent
                {
                    Id = ++maxPCId,
                    ShipId = element.Id,
                    ComponentId = pc.Key,
                    Count = pc.Value.Item2
                });
            }
        }
        public void Delete(ShipBindingModel model)
        {
            // удаяем записи по компонентам при удалении изделия
            source.ShipComponents.RemoveAll(rec => rec.ShipId == model.Id);
            Ship element = source.Ships.FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                source.Ships.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
        public List<ShipViewModel> Read(ShipBindingModel model)
        {
            return source.Ships
            .Where(rec => model == null || rec.Id == model.Id)
            .Select(rec => new ShipViewModel
            {
                Id = rec.Id,
                ShipName = rec.ShipName,
                Price = rec.Price,
                ShipComponents = source.ShipComponents
            .Where(recPC => recPC.ShipId == rec.Id)
            .ToDictionary(recPC => recPC.ComponentId, recPC =>
            (source.Components.FirstOrDefault(recC => recC.Id == recPC.ComponentId)?.ComponentName, recPC.Count))
            })
            .ToList();
        }
        /*
        private readonly FileDataListSingleton source;
        public ShipLogic()
        {
            source = FileDataListSingleton.GetInstance();
        }
        public void CreateOrUpdate(ShipBindingModel model)
        {
            Ship tempShip = model.Id.HasValue ? null : new Ship { Id = 1 };
            foreach (var ship in source.Ships)
            {
                if (ship.ShipName == model.ShipName && ship.Id != model.Id)
                {
                    throw new Exception("Уже есть мебель с таким названием");
                }
                if (!model.Id.HasValue && ship.Id >= tempShip.Id)
                {
                    tempShip.Id = ship.Id + 1;
                }
                else if (model.Id.HasValue && ship.Id == model.Id)
                {
                    tempShip = ship;
                }
            }
            if (model.Id.HasValue)
            {
                if (tempShip == null)
                {
                    throw new Exception("Элемент не найден");
                }
                CreateModel(model, tempShip);
            }
            else
            {
                source.Ships.Add(CreateModel(model, tempShip));
            }
        }
        public void Delete(ShipBindingModel model)
        {
            for (int i = 0; i < source.ShipComponents.Count; ++i)
            {
                if (source.ShipComponents[i].ShipId == model.Id)
                {
                    source.ShipComponents.RemoveAt(i--);
                }
            }
            for (int i = 0; i < source.Ships.Count; ++i)
            {
                if (source.Ships[i].Id == model.Id)
                {
                    source.Ships.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
        private Ship CreateModel(ShipBindingModel model, Ship mebel)
        {
            mebel.ShipName = model.ShipName;
            mebel.Price = model.Price;
            int maxPCId = 0;
            for (int i = 0; i < source.ShipComponents.Count; ++i)
            {
                if (source.ShipComponents[i].Id > maxPCId)
                {
                    maxPCId = source.ShipComponents[i].Id;
                }
                if (source.ShipComponents[i].ShipId == mebel.Id)
                {
                    if
                    (model.ShipComponents.ContainsKey(source.ShipComponents[i].ComponentId))
                    {
                        source.ShipComponents[i].Count =
                        model.ShipComponents[source.ShipComponents[i].ComponentId].Item2;
                        model.ShipComponents.Remove(source.ShipComponents[i].ComponentId);
                    }
                    else
                    {
                        source.ShipComponents.RemoveAt(i--);
                    }
                }
            }
            foreach (var pc in model.ShipComponents)
            {
                source.ShipComponents.Add(new ShipComponent
                {
                    Id = ++maxPCId,
                    ShipId = mebel.Id,
                    ComponentId = pc.Key,
                    Count = pc.Value.Item2
                });
            }
            return mebel;
        }
        public List<ShipViewModel> Read(ShipBindingModel model)
        {
            List<ShipViewModel> result = new List<ShipViewModel>();
            foreach (var component in source.Ships)
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
        private ShipViewModel CreateViewModel(Ship ship)
        {
            Dictionary<int, (string, int)> shipComponents = new Dictionary<int,(string, int)>();
            foreach (var pc in source.ShipComponents)
            {
                if (pc.ShipId == ship.Id)
                {
                    string componentName = string.Empty;
                    foreach (var component in source.Components)
                    {
                        if (pc.ComponentId == component.Id)
                        {
                            componentName = component.ComponentName;
                            break;
                        }
                    }
                    shipComponents.Add(pc.ComponentId, (componentName, pc.Count));
                }
            }
            return new ShipViewModel
            {
                Id = ship.Id,
                ShipName = ship.ShipName,
                Price = ship.Price,
                ShipComponents = shipComponents
            };
        }
        */
    }
}
