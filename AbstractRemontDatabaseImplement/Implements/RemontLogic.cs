using AbstractRemontBusinessLogic.BindingModels;
using AbstractRemontBusinessLogic.Interfaces;
using AbstractRemontBusinessLogic.ViewModels;
using AbstractRemontDatabaseImplement.Models;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbstractRemontBusinessLogic.Enums;

namespace AbstractRemontDatabaseImplement.Implements
{
    public class RemontLogic : IRemontLogic
    {
        private readonly AbstractRemontDatabase source;
        public void CreateOrUpdate(RemontBindingModel model)
        {
            using (var context = new AbstractRemontDatabase())
            {
                Remont element;

                if (model.Id.HasValue)
                {
                    element = context.Remonts.FirstOrDefault(rec => rec.Id == model.Id);

                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                }
                else
                {
                    element = new Remont();
                    context.Remonts.Add(element);
                }

                element.ShipId = model.ShipId == 0 ? element.ShipId : model.ShipId;
                element.ClientId = model.ClientId.Value;
                element.ImplementerId = model.ImplementerId;
                element.Count = model.Count;
                element.Sum = model.Sum;
                element.Status = model.Status;
                element.DateCreate = model.DateCreate;
                element.DateImplement = model.DateImplement;

                context.SaveChanges();
            }
        }

        public void Delete(RemontBindingModel model)
        {
            using (var context = new AbstractRemontDatabase())
            {
                Remont element = context.Remonts.FirstOrDefault(rec => rec.Id == model.Id);

                if (element != null)
                {
                    context.Remonts.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        public List<RemontViewModel> Read(RemontBindingModel model)
        {
            using (var context = new AbstractRemontDatabase())
            {
                return context.Remonts
               .Where(
                   rec => model == null
                   || rec.Id == model.Id && model.Id.HasValue
                   || model.DateFrom.HasValue && model.DateTo.HasValue && rec.DateCreate >= model.DateFrom && rec.DateCreate <= model.DateTo
                   || model.ClientId.HasValue && rec.ClientId == model.ClientId
                   || model.FreeOrders.HasValue && model.FreeOrders.Value && !rec.ImplementerId.HasValue
                   || model.ImplementerId.HasValue && rec.ImplementerId == model.ImplementerId && rec.Status == RemontStatus.Выполняется
               )
               .Include(rec => rec.Ship)
               .Include(rec => rec.Client)
               .Include(rec => rec.Implementer)
               .Select(rec => new RemontViewModel
               {
                   Id = rec.Id,
                   ClientId = rec.ClientId,
                   ImplementerId = rec.ImplementerId,
                   ShipId = rec.ShipId,
                   Count = rec.Count,
                   Sum = rec.Sum,
                   Status = rec.Status,
                   DateCreate = rec.DateCreate,
                   DateImplement = rec.DateImplement,
                   ShipName = rec.Ship.ShipName,
                   ClientFIO = rec.Client.FIO,
                   ImplementerFIO = rec.ImplementerId.HasValue ? rec.Implementer.ImplementerFIO : string.Empty,
               })
               .ToList();
            }
        }
    }
}
