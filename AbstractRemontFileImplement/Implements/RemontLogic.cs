using AbstractRemontBusinessLogic.BindingModels;
using AbstractRemontBusinessLogic.Enums;
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
    public class RemontLogic : IRemontLogic
    {
        private readonly FileDataListSingleton source;
        public RemontLogic()
        {
            source = FileDataListSingleton.GetInstance();
        }
        public void CreateOrUpdate(RemontBindingModel model)
        {
            Remont order;
            if (model.Id.HasValue)
            {
                order = source.Remonts.FirstOrDefault(rec => rec.Id == model.Id);
                if (order == null)
                    throw new Exception("Элемент не найден");
            }
            else
            {
                int maxId = source.Remonts.Count > 0 ? source.Remonts.Max(rec => rec.Id) : 0;
                order = new Remont { Id = maxId + 1 };
                source.Remonts.Add(order);
            }
            order.ShipId = model.ShipId;
            order.ClientFIO = model.ClientFIO;
            order.ClientId = model.ClientId;
            order.Count = model.Count;
            order.DateCreate = model.DateCreate;
            order.DateImplement = model.DateImplement;
            order.Status = model.Status;
            order.Sum = model.Sum;
        }

        public void Delete(RemontBindingModel model)
        {
            Remont order = source.Remonts.FirstOrDefault(rec => rec.Id == model.Id);
            if (order != null)
            {
                source.Remonts.Remove(order);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }

        public List<RemontViewModel> Read(RemontBindingModel model)
        {
            return source.Remonts
            .Where(rec => model == null || rec.Id == model.Id)
            .Select(rec => new RemontViewModel
            {
                Id = rec.Id,
                ShipId = rec.ShipId,
                ShipName = source.Ships.FirstOrDefault((r) => r.Id == rec.ShipId).ShipName,
                ClientFIO = rec.ClientFIO,
                ClientId = rec.ClientId,
                Count = rec.Count,
                DateCreate = rec.DateCreate,
                DateImplement = rec.DateImplement,
                Status = rec.Status,
                Sum = rec.Sum
            }).ToList();
        }
    }
}
