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

namespace AbstractRemontDatabaseImplement.Implements
{
    public class RemontLogic : IRemontLogic
    {
        private readonly AbstractRemontDatabase source;
        public void CreateOrUpdate(RemontBindingModel model)
        {
            using (var context = new AbstractRemontDatabase())
            {
                Remont order;
                if (model.Id.HasValue)
                {
                    order = context.Remonts.ToList().FirstOrDefault(rec => rec.Id == model.Id);
                    if (order == null)
                        throw new Exception("Элемент не найден");
                }
                else
                {
                    order = new Remont();
                    context.Remonts.Add(order);
                }
                order.ShipId = model.ShipId;
                order.ClientFIO = model.ClientFIO;
                order.ClientId = model.ClientId;
                order.Count = model.Count;
                order.DateCreate = model.DateCreate;
                order.DateImplement = model.DateImplement;
                order.Status = model.Status;
                order.Sum = model.Sum;
                context.SaveChanges();
            }
        }

        public void Delete(RemontBindingModel model)
        {
            using (var context = new AbstractRemontDatabase())
            {
                Remont order = context.Remonts.FirstOrDefault(rec => rec.Id == model.Id);
                if (order != null)
                {
                    context.Remonts.Remove(order);
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
                context.SaveChanges();
            }
        }

        public List<RemontViewModel> Read(RemontBindingModel model)
        {
            using (var context = new AbstractRemontDatabase())
            {
                return context.Remonts.Where(rec => model == null || rec.Id == model.Id || (rec.DateCreate >= model.DateFrom)
                && (rec.DateCreate <= model.DateTo) || model.ClientId == rec.ClientId)
                .Include(ord => ord.Ship)
                .Select(rec => new RemontViewModel()
                {
                    Id = rec.Id,
                    ShipId = rec.ShipId,
                    ClientFIO = rec.ClientFIO,
                    ClientId = rec.ClientId,
                    ShipName = rec.Ship.ShipName,
                    Count = rec.Count,
                    DateCreate = rec.DateCreate,
                    DateImplement = rec.DateImplement,
                    Status = rec.Status,
                    Sum = rec.Sum
                }).ToList();
            }
        }
    }
}
