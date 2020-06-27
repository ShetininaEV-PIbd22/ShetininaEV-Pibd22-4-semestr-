using AbstractRemontBusinessLogic.BindingModels;
using AbstractRemontBusinessLogic.Enums;
using AbstractRemontBusinessLogic.Interfaces;
using System;

namespace AbstractRemontBusinessLogic.BusinessLogics
{
    // Создание заказа и смена его статусов
    public class MainLogic
    {
        private readonly IRemontLogic remontLogic;
        private readonly ISkladLogic skladLogic;

        public MainLogic(IRemontLogic remontLogic, ISkladLogic skladLogic)
        {
            this.remontLogic = remontLogic;
            this.skladLogic = skladLogic;
        }

        public void CreateRemont(CreateRemontBindingModel model)
        {
            remontLogic.CreateOrUpdate(new RemontBindingModel
            {
                ShipId = model.ShipId,
                Count = model.Count,
                Sum = model.Sum,
                DateCreate = DateTime.Now,
                Status = RemontStatus.Принят
            });
        }

        public void TakeRemontInWork(ChangeStatusBindingModel model)
        {
            var order = remontLogic.Read(new RemontBindingModel 
            { 
                Id = model.RemontId 
            })?[0];

            if (order == null)
            {
                throw new Exception("Не найден заказ");
            }

            if (order.Status != RemontStatus.Принят)
            {
                throw new Exception("Заказ не в статусе \"Принят\"");
            }

            remontLogic.CreateOrUpdate(new RemontBindingModel
            {
                Id = order.Id,
                ShipId = order.ShipId,
                Count = order.Count,
                Sum = order.Sum,
                DateCreate = order.DateCreate,
                Status = RemontStatus.Выполняется
            });
        }

        public void FinishRemont(ChangeStatusBindingModel model)
        {
            var order = remontLogic.Read(new RemontBindingModel 
            { 
                Id = model.RemontId 
            })?[0];

            if (order == null)
            {
                throw new Exception("Не найден заказ");
            }

            if (order.Status != RemontStatus.Выполняется)
            {
                throw new Exception("Заказ не в статусе \"Выполняется\"");
            }

            remontLogic.CreateOrUpdate(new RemontBindingModel
            {
                Id = order.Id,
                ShipId = order.ShipId,
                Count = order.Count,
                Sum = order.Sum,
                DateCreate = order.DateCreate,
                DateImplement = DateTime.Now,
                Status = RemontStatus.Готов
            });
        }

        public void PayRemont(ChangeStatusBindingModel model)
        {
            var order = remontLogic.Read(new RemontBindingModel 
            { 
                Id = model.RemontId 
            })?[0];

            if (order == null)
            {
                throw new Exception("Не найден заказ");
            }

            if (order.Status != RemontStatus.Готов)
            {
                throw new Exception("Заказ не в статусе \"Готов\"");
            }

            remontLogic.CreateOrUpdate(new RemontBindingModel
            {
                Id = order.Id,
                ShipId = order.ShipId,
                Count = order.Count,
                Sum = order.Sum,
                DateCreate = order.DateCreate,
                DateImplement = order.DateImplement,
                Status = RemontStatus.Оплачен
            });
        }

        public void AddComponents(SkladComponentBindingModel model)
        {
            skladLogic.AddComponent(model);
        }
    }
}
