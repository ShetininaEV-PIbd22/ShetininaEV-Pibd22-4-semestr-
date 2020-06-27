using AbstractRemontBusinessLogic.BindingModels;
using AbstractRemontBusinessLogic.Enums;
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
            {
                source.Remonts.Add(CreateModel(model, tempOrder));
            }
        }

        public void Delete(RemontBindingModel model)
        {
            for (int i = 0; i < source.Remonts.Count; ++i)
            {
                if (source.Remonts[i].Id == model.Id)
                {
                    source.Remonts.RemoveAt(i);
                    return;
                }
            }

            throw new Exception("Элемент не найден");
        }

        private Remont CreateModel(RemontBindingModel model, Remont order)
        {
            order.ShipId = model.ShipId;
            order.Count = model.Count;
            order.ClientId = (int)model.ClientId;
            order.DateCreate = model.DateCreate;
            order.DateImplement = model.DateImplement;
            order.Sum = model.Sum;
            order.Status = model.Status;

            return order;
        }

        public List<RemontViewModel> Read(RemontBindingModel model)
        {
            List<RemontViewModel> result = new List<RemontViewModel>();

            foreach (var order in source.Remonts)
            {
                if (
                    model != null && order.Id == model.Id
                    || model.DateFrom.HasValue && model.DateTo.HasValue && order.DateCreate >= model.DateFrom && order.DateCreate <= model.DateTo
                    //|| model.ClientId.HasValue && order.ClientId == model.ClientId
                    || model.FreeOrders.HasValue && model.FreeOrders.Value
                    //|| model.ImplementerId.HasValue && order.ImplementerId == model.ImplementerId && order.Status == RemontStatus.Выполняется
                )
                {
                    result.Add(CreateViewModel(order));
                    break;
                }

                result.Add(CreateViewModel(order));
            }

            return result;
        }

        private RemontViewModel CreateViewModel(Remont order)
        {
            string productName = null;

            foreach (var product in source.Ships)
            {
                if (product.Id == order.ShipId)
                {
                    productName = product.ShipName;
                }
            }

            if (productName == null)
            {
                throw new Exception("Продукт не найден");
            }

            return new RemontViewModel
            {
                Id = order.Id,
                ClientId = order.ClientId,
                ShipId = order.ShipId,
                ShipName = productName,
                Count = order.Count,
                Sum = order.Sum,
                Status = order.Status,
                DateCreate = order.DateCreate,
                DateImplement = order.DateImplement
            };
        }
    }
}
