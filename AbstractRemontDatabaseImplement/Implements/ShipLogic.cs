using AbstractRemontBusinessLogic.BindingModels;
using AbstractRemontBusinessLogic.Interfaces;
using AbstractRemontBusinessLogic.ViewModels;
using AbstractRemontDatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace AbstractRemontDatabaseImplement.Implements
{
    public class ShipLogic : IShipLogic
    {
        public void CreateOrUpdate(ShipBindingModel model)
        {
            using (var context = new AbstractRemontDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Ship element = context.Ships.FirstOrDefault(rec => rec.ShipName == model.ShipName && rec.Id != model.Id);
                        if (element != null)
                        {
                            throw new Exception("Уже есть корабль с таким названием");
                        }
                        if (model.Id.HasValue)
                        {
                            element = context.Ships.FirstOrDefault(rec => rec.Id == model.Id);

                            if (element == null)
                            {
                                throw new Exception("Элемент не найден");
                            }
                        }
                        else
                        {
                            element = new Ship();
                            context.Ships.Add(element);
                        }

                        element.ShipName = model.ShipName;
                        element.Price = model.Price;
                        context.SaveChanges();
                        if (model.Id.HasValue)
                        {
                            var shipComponents = context.ShipComponents.Where(rec => rec.ShipId == model.Id.Value).ToList();
                            context.ShipComponents.RemoveRange(shipComponents.Where(rec => !model.ShipComponents.ContainsKey(rec.ComponentId)).ToList());
                            context.SaveChanges();
                            foreach (var updateComponent in shipComponents)
                            {
                                updateComponent.Count =
                                model.ShipComponents[updateComponent.ComponentId].Item2;
                                model.ShipComponents.Remove(updateComponent.ComponentId);
                            }
                            context.SaveChanges();
                        }
                        foreach (var pc in model.ShipComponents)
                        {
                            context.ShipComponents.Add(new ShipComponent
                            {
                                ShipId = element.Id,
                                ComponentId = pc.Key,
                                Count = pc.Value.Item2
                            });
                            context.SaveChanges();
                        }
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public void Delete(ShipBindingModel model)
        {
            using (var context = new AbstractRemontDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.ShipComponents.RemoveRange(context.ShipComponents.Where(rec => rec.ShipId == model.Id));
                        Ship element = context.Ships.FirstOrDefault(rec => rec.Id == model.Id);
                        if (element != null)
                        {
                            context.Ships.Remove(element);
                            context.SaveChanges();
                        }
                        else
                        {
                            throw new Exception("Элемент не найден");
                        }
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public List<ShipViewModel> Read(ShipBindingModel model)
        {
            using (var context = new AbstractRemontDatabase())
            {
                return context.Ships
                .Where(rec => model == null || rec.Id == model.Id)
                .ToList()
                .Select(rec => new ShipViewModel
                {
                    Id = rec.Id,
                    ShipName = rec.ShipName,
                    Price = rec.Price,
                    ShipComponents = context.ShipComponents
                .Include(recPC => recPC.Component)
                .Where(recPC => recPC.ShipId == rec.Id)
                .ToDictionary(recPC => recPC.ComponentId, recPC =>
                (recPC.Component?.ComponentName, recPC.Count))
                })
                .ToList();
            }
        }

    }
}
