using AbstractRemontBusinessLogic.BindingModels;
using AbstractRemontBusinessLogic.Enums;
using AbstractRemontBusinessLogic.HelperModels;
using AbstractRemontBusinessLogic.Interfaces;
using AbstractRemontBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractRemontBusinessLogic.BusinessLogics
{
    // Создание заказа и смена его статусов
    public class MainLogic
    {

        private readonly IRemontLogic orderLogic;

        private readonly object locker = new object();

        public MainLogic(IRemontLogic orderLogic)
        {
            this.orderLogic = orderLogic;
        }

        public void CreateRemont(CreateRemontBindingModel model)
        {
            orderLogic.CreateOrUpdate(new RemontBindingModel
            {
                ShipId = model.ShipId,
                ClientId = model.ClientId,
                Count = model.Count,
                Sum = model.Sum,
                DateCreate = DateTime.Now,
                Status = RemontStatus.Принят
            });
        }

        public void TakeRemontInWork(ChangeStatusBindingModel model)
        {
            lock (locker)
            {
                var order = orderLogic.Read(new RemontBindingModel { Id = model.RemontId })?[0];

                if (order == null)
                {
                    throw new Exception("Не найден заказ");
                }

                if (order.Status != RemontStatus.Принят)
                {
                    throw new Exception("Заказ не в статусе \"Принят\"");
                }

                if (order.ImplementerId.HasValue)
                {
                    throw new Exception("У заказа уже есть исполнитель");
                }

                orderLogic.CreateOrUpdate(new RemontBindingModel
                {
                    Id = order.Id,
                    ClientId = order.ClientId,
                    ShipId = order.ShipId,
                    ImplementerId = model.ImplementerId,
                    Count = order.Count,
                    Sum = order.Sum,
                    DateCreate = order.DateCreate,
                    Status = RemontStatus.Выполняется
                });
            }
        }

        public void FinishRemont(ChangeStatusBindingModel model)
        {
            var order = orderLogic.Read(new RemontBindingModel { Id = model.RemontId })?[0];

            if (order == null)
            {
                throw new Exception("Не найден заказ");
            }

            if (order.Status != RemontStatus.Выполняется)
            {
                throw new Exception("Заказ не в статусе \"Выполняется\"");
            }

            orderLogic.CreateOrUpdate(new RemontBindingModel
            {
                Id = order.Id,
                ClientId = order.ClientId,
                ShipId = order.ShipId,
                ImplementerId = order.ImplementerId,
                Count = order.Count,
                Sum = order.Sum,
                DateCreate = order.DateCreate,
                DateImplement = DateTime.Now,
                Status = RemontStatus.Готов
            });
        }

        public void PayRemont(ChangeStatusBindingModel model)
        {
            var order = orderLogic.Read(new RemontBindingModel { Id = model.RemontId })?[0];

            if (order == null)
            {
                throw new Exception("Не найден заказ");
            }

            if (order.Status != RemontStatus.Готов)
            {
                throw new Exception("Заказ не в статусе \"Готов\"");
            }

            orderLogic.CreateOrUpdate(new RemontBindingModel
            {
                Id = order.Id,
                ClientId = order.ClientId,
                ShipId = order.ShipId,
                ImplementerId = model.ImplementerId,
                Count = order.Count,
                Sum = order.Sum,
                DateCreate = order.DateCreate,
                DateImplement = order.DateImplement,
                Status = RemontStatus.Оплачен
            });
        }
    }
}
