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
                int maxId = source.Remonts.Count > 0 ? source.Remonts.Max(rec =>
               rec.Id) : 0;
                element = new Remont { Id = maxId + 1 };
                source.Remonts.Add(element);
            }
            element.ShipId = model.ShipId == 0 ? element.ShipId : model.ShipId;
            element.Count = model.Count;
            element.Sum = model.Sum;
            element.Status = model.Status;
            element.DateCreate = model.DateCreate;
            element.DateImplement = model.DateImplement;
        }
        public void Delete(RemontBindingModel model)
        {
            // удаяем записи по компонентам при удалении изделия
            Remont element = source.Remonts.FirstOrDefault(rec => rec.Id ==model.Id);
            if (element != null)
            {
                source.Remonts.Remove(element);
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
                ShipName=getName(rec.ShipId),
                Count = rec.Count,
                Sum = rec.Sum,
                Status = rec.Status,
                DateCreate = rec.DateCreate,
                DateImplement = rec.DateImplement
            })
            .ToList();
        }

        private string getName(int id)
        {
            string ShipName = "";
            for (int j = 0; j < source.Ships.Count; ++j)
            {
                if (source.Ships[j].Id == id)
                {
                    ShipName = source.Ships[j].ShipName;
                    break;
                }
            }
            return ShipName;
        }
    }
}
