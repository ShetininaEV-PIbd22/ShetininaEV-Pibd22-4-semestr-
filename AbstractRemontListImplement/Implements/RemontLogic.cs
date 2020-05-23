using AbstractRemontBusinessLogic.BindingModels;
using AbstractRemontBusinessLogic.Interfaces;
using AbstractRemontBusinessLogic.ViewModels;
using AbstractRemontListImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractRemontListImplement.Implements
{
    public class RemontLogic : IRemontLogic
    {
        private readonly DataListSingleton source;

        public RemontLogic()
        {
            source = DataListSingleton.GetInstance();
        }

        public void CreateOrUpdate(RemontBindingModel model)
        {
            Remont tempOrder = model.Id.HasValue ? null : new Remont
            {
                Id = 1
            };
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
            {
                source.Remonts.Add(CreateModel(model, tempOrder));
            }
        }

        public void Delete(RemontBindingModel model)
        {
            for (int i = 0; i < source.Remonts.Count; ++i)
            {
                if (source.Remonts[i].Id == model.Id.Value)
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
                    if (order.Id == model.Id && order.ClientId == model.ClientId)
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
            order.Count = model.Count;
            order.ClientId = model.ClientId;
            order.ClientFIO = model.ClientFIO;
            order.DateCreate = model.DateCreate;
            order.DateImplement = model.DateImplement;
            order.ShipId = model.ShipId;
            order.Status = model.Status;
            order.Sum = model.Sum;
            return order;
        }

        private RemontViewModel CreateViewModel(Remont order)
        {
            var shipName = source.Ships.FirstOrDefault((n) => n.Id == order.ShipId).ShipName;
            return new RemontViewModel
            {
                Id = order.Id,
                Count = order.Count,
                ClientId = order.ClientId,
                ClientFIO = order.ClientFIO,
                DateCreate = order.DateCreate,
                DateImplement = order.DateImplement,
                ShipName = shipName,
                ShipId = order.ShipId,
                Status = order.Status,
                Sum = order.Sum
            };
        }
    }
}
