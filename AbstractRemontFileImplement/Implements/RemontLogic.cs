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
            Remont element;

            if (model.Id.HasValue)
            {
                element = source.Remonts.FirstOrDefault(rec => rec.Id == model.Id);

                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
            }
            else
            {
                int maxId = source.Remonts.Count > 0 ? source.Remonts.Max(rec => rec.Id) : 0;
                element = new Remont { Id = maxId + 1 };
                source.Remonts.Add(element);
            }

            element.ShipId = model.ShipId == 0 ? element.ShipId : model.ShipId;
            element.ClientId = model.ClientId.Value;
            element.ImplementerId = model.ImplementerId;
            element.Count = model.Count;
            element.Sum = model.Sum;
            element.Status = model.Status;
            element.DateCreate = model.DateCreate;
            element.DateImplement = model.DateImplement;
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
             .Where(
                 rec => model == null
                 || rec.Id == model.Id
                 || model.DateFrom.HasValue && model.DateTo.HasValue && rec.DateCreate >= model.DateFrom && rec.DateCreate <= model.DateTo
                 || model.ClientId.HasValue && rec.ClientId == model.ClientId
                 || model.FreeOrders.HasValue && model.FreeOrders.Value && !rec.ImplementerId.HasValue
                 || model.ImplementerId.HasValue && rec.ImplementerId == model.ImplementerId && rec.Status == RemontStatus.Выполняется
             )
             .Select(rec => new RemontViewModel
             {
                 Id = rec.Id,
                 ClientId = rec.ClientId,
                 ImplementerId = rec.ImplementerId,
                 ShipId = rec.ShipId,
                 ClientFIO = source.Clients.FirstOrDefault(recC => recC.Id == rec.ClientId)?.FIO,
                 ImplementerFIO = source.Implementers.FirstOrDefault(recC => recC.Id == rec.ImplementerId)?.ImplementerFIO,
                 ShipName = source.Ships.FirstOrDefault(recP => recP.Id == rec.ShipId)?.ShipName,
                 Count = rec.Count,
                 Sum = rec.Sum,
                 Status = rec.Status,
                 DateCreate = rec.DateCreate,
                 DateImplement = rec.DateImplement
             })
             .ToList();
        }
    }
}
