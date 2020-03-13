using AbstractRemontBusinessLogic.BindingModels;
using AbstractRemontBusinessLogic.Enums;
using AbstractRemontBusinessLogic.Interfaces;
using AbstractRemontBusinessLogic.ViewModels;
using AbstractRemontListImplement.Models;
using System;
using System.Collections.Generic;

namespace AbstractRemontListImplement.Implements
{
    public class MainLogic
    {
        private readonly DataListSingleton source;

        public MainLogic()
        {
            source = DataListSingleton.GetInstance();
        }

        public void CreateOrder(RemontBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Remonts.Count; ++i)
            {
                if (source.Remonts[i].Id > maxId)
                {
                    maxId = source.Remonts[i].Id;
                }
            }
            source.Remonts.Add(new Remont
            {
                Id = maxId + 1,
                ShipId = model.ShipId,
                DateCreate = DateTime.Now,
                Count = model.Count,
                Sum = model.Sum,
                Status = RemontStatus.Принят
            });
        }

        public void FinishOrder(RemontBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Remonts.Count; ++i)
            {
                if (source.Remonts[i].Id == model.Id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Заявка на ремонт не найдена");
            }
            if (source.Remonts[index].Status != RemontStatus.Принят)
            {
                throw new Exception("Ремонт не в статусе \"Принят\"");
            }
            source.Remonts[index].Status = RemontStatus.Выполняется;
        }

        public List<RemontViewModel> GetOrders()
        {
            List<RemontViewModel> result = new List<RemontViewModel>();
            for (int i = 0; i < source.Remonts.Count; ++i)
            {
                string productName = string.Empty;
                for (int j = 0; j < source.Ships.Count; ++j)
                {
                    if (source.Ships[j].Id == source.Remonts[i].ShipId)
                    {
                        productName = source.Ships[j].ShipName;
                        break;
                    }
                }
                result.Add(new RemontViewModel
                {
                    Id = source.Remonts[i].Id,
                    ShipId = source.Remonts[i].ShipId,
                    ShipName = productName,
                    Count = source.Remonts[i].Count,
                    Sum = source.Remonts[i].Sum,
                    DateCreate = source.Remonts[i].DateCreate,
                    DateImplement = source.Remonts[i].DateImplement,
                    Status = source.Remonts[i].Status
                });
            }
            return result;
        }

        public void PayOrder(RemontBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Remonts.Count; ++i)
            {
                if (source.Remonts[i].Id == model.Id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Заявка на ремонт не найдена");
            }
            if (source.Remonts[index].Status != RemontStatus.Готов)
            {
                throw new Exception("Ремонт не в статусе \"Готов\"");
            }
            source.Remonts[index].Status = RemontStatus.Оплачен;
        }

        public void TakeOrderInWork(RemontBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Remonts.Count; ++i)
            {
                if (source.Remonts[i].Id == model.Id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Заявка на ремонт не найдена");
            }
            if (source.Remonts[index].Status != RemontStatus.Принят)
            {
                throw new Exception("Заказ не в статусе \"Принят\"");
            }
            source.Remonts[index].DateImplement = DateTime.Now;
            source.Remonts[index].Status = RemontStatus.Выполняется;
        }
    }
}
