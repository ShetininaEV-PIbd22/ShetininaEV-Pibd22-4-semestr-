using AbstractRemontBusinessLogic.BindingModels;
using AbstractRemontBusinessLogic.Interfaces;
using AbstractRemontBusinessLogic.ViewModels;
using AbstractRemontFileImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
    }
}
