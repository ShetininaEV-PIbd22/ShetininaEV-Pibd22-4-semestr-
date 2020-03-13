using AbstractRemontBusinessLogic.BindingModels;
using AbstractRemontBusinessLogic.Interfaces;
using AbstractRemontBusinessLogic.ViewModels;
using AbstractRemontListImplement.Models;
using System;
using System.Collections.Generic;

namespace AbstractRemontListImplement.Implements
{
    public class ShipLogic : IShipLogic
    {
        private readonly DataListSingleton source;

        public ShipLogic()
        {
            source = DataListSingleton.GetInstance();
        }

        public void CreateOrUpdate(ShipBindingModel model)
        {
            Ship tempShip = model.Id.HasValue ? null : new Ship { Id = 1 };
            foreach (var ship in source.Ships)
            {
                if (ship.ShipName == model.ShipName && ship.Id != model.Id)
                    throw new Exception("Уже есть корабль с таким названием");
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
                source.Ships.Add(CreateModel(model, tempShip));
        }

        public void Delete(ShipBindingModel model)
        {
            // удаляем записи по ингредиентам при удалении изделия
            for (int i = 0; i < source.ShipComponents.Count; ++i)
                if (source.ShipComponents[i].ProductId == model.Id)
                    source.ShipComponents.RemoveAt(i--);
            for (int i = 0; i < source.Ships.Count; ++i)
                if (source.Ships[i].Id == model.Id)
                {
                    source.Ships.RemoveAt(i);
                    return;
                }
            throw new Exception("Элемент не найден");
        }

        public List<ShipViewModel> Read(ShipBindingModel model)
        {
            List<ShipViewModel> result = new List<ShipViewModel>();
            foreach (var ship in source.Ships)
            {
                if (model != null)
                {
                    if (ship.Id == model.Id)
                    {
                        result.Add(CreateViewModel(ship));
                        break;
                    }
                    continue;
                }
                result.Add(CreateViewModel(ship));
            }
            return result;
        }

        private Ship CreateModel(ShipBindingModel model, Ship product)
        {
            product.ShipName = model.ShipName;
            product.Price = model.Price;
            //обновляем существуюущие ингредиенты и ищем максимальный идентификатор
            int maxPCId = 0;
            for (int i = 0; i < source.ShipComponents.Count; ++i)
            {
                if (source.ShipComponents[i].Id > maxPCId)
                {
                    maxPCId = source.ShipComponents[i].Id;
                }
                if (source.ShipComponents[i].ProductId == product.Id)
                {
                    // если в модели пришла запись ингредиента с таким id
                    if (model.ShipComponents.ContainsKey(source.ShipComponents[i].IngredientId))
                    {
                        // обновляем количество
                        source.ShipComponents[i].Count =
                            model.ShipComponents[source.ShipComponents[i].IngredientId].Item2;
                        // из модели убираем эту запись, чтобы остались только не просмотренные
                        model.ShipComponents.Remove(source.ShipComponents[i].IngredientId);
                    }
                    else
                        source.ShipComponents.RemoveAt(i--);
                }
            }
            // новые записи
            foreach (var pi in model.ShipComponents)
            {
                source.ShipComponents.Add(new ShipComponents
                {
                    Id = ++maxPCId,
                    ProductId = product.Id,
                    IngredientId = pi.Key,
                    Count = pi.Value.Item2
                });
            }
            return product;
        }

        private ShipViewModel CreateViewModel(Ship product)
        {
            // требуется дополнительно получить список компонентов для изделия с названиями и их количество
            Dictionary<int, (string, int)> productIngredients = new Dictionary<int, (string, int)>();
            foreach (var pi in source.ShipComponents)
                if (pi.ProductId == product.Id)
                {
                    string componentName = string.Empty;
                    foreach (var component in source.Components)
                        if (pi.IngredientId == component.Id)
                        {
                            componentName = component.ComponentName;
                            break;
                        }
                    productIngredients.Add(pi.IngredientId, (componentName, pi.Count));
                }
            return new ShipViewModel
            {
                Id = product.Id,
                ShipName = product.ShipName,
                Price = product.Price,
                ShipComponents = productIngredients
            };
        }
    }
}
