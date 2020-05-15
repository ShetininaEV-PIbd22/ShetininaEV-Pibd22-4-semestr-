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
                .Select(rec => new RemontViewModel
                {
                    Id = rec.Id,
                    ShipId = rec.ShipId,
                    ShipName = rec.Ship.ShipName,
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
}
