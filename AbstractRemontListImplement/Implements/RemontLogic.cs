using AbstractRemontBusinessLogic.BindingModels;
using AbstractRemontBusinessLogic.Interfaces;
using AbstractRemontBusinessLogic.ViewModels;
using AbstractRemontListImplement.Models;
using System;
using System.Collections.Generic;

namespace AbstractRemontListImplement.Implements
{
    public class RemontLogic : IRemontLogic
    {
        private readonly DataListSingleton source;

        public RemontLogic() {
            source = DataListSingleton.GetInstance();
        }

        public void CreateOrUpdate(RemontBindingModel model)
        {
            Remont tempOrder = model.Id.HasValue ? null : new Remont { Id = 1 };
            foreach (var order in source.Remonts)
            {
                if (!model.Id.HasValue && order.Id >= tempOrder.Id)
                {
                    tempOrder.Id = order.Id + 1;
                }
                else if (model.Id.HasValue && order.Id == model.Id)
                {
                    tempOrder = order;
                }
            }
            if (model.Id.HasValue)
            {
                if (tempOrder == null)
                {
                    throw new Exception("Элемент не найден");
                }
                CreateModel(model, tempOrder);
            }
           else
               source.Remonts.Add(CreateModel(model, tempOrder));
        }

        public void Delete(RemontBindingModel model)
        {
            // удаляем записи по ингредиентам и изделиям при удалении заказа
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
                    source.Ships.RemoveAt(i--);
                }
            }
            for (int i = 0; i < source.Ships.Count; ++i)
                if (source.Remonts[i].Id == model.Id)
                {
                    source.Remonts.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }

        public List<RemontViewModel> Read(RemontBindingModel model)
        {
            List<RemontViewModel> result = new List<RemontViewModel>();
            foreach (var order in source.Remonts)
            {
                if (model != null)
                {
                    if (order.Id == model.Id)
                    {
                        result.Add(CreateViewModel(order));
                        break;
                    }
                    continue;
                }
                result.Add(CreateViewModel(order));
            }
            return result;
        }

        private Remont CreateModel(RemontBindingModel model, Remont order)
        {
            order.ShipId = model.ShipId;
            order.Sum = model.Sum;
            order.DateCreate = model.DateCreate;
            order.Count = model.Count;
            order.DateImplement = model.DateImplement;
            order.Status = model.Status;
            return order;
        }

        private RemontViewModel CreateViewModel(Remont order)
        {
            string shipName = "";
            foreach (var product in source.Ships)
            {
                if (product.Id == order.ShipId)
                {
                    shipName = product.ShipName;
                }
            }
            return new RemontViewModel
            {
                Id = order.Id,
                ShipId = order.ShipId,
                Count = order.Count,
                Sum = order.Sum,
                Status = order.Status,
                ShipName = shipName,
                DateCreate = order.DateCreate,
                DateImplement = order.DateImplement
            };
        }
    }
}
