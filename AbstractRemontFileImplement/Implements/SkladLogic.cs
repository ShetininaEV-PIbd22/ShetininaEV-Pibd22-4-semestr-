using AbstractRemontBusinessLogic.BindingModels;
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
    public class SkladLogic : ISkladLogic
    {
        private readonly FileDataListSingleton source;

        public SkladLogic()
        {
            source = FileDataListSingleton.GetInstance();
        }

        public void CreateOrUpdate(SkladBindingModel model)
        {
            Sklad element = source.Sklads.FirstOrDefault(rec => rec.SkladName == model.SkladName && rec.Id != model.Id);

            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }

            if (model.Id.HasValue)
            {
                element = source.Sklads.FirstOrDefault(rec => rec.Id == model.Id);

                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
            }
            else
            {
                int maxId = source.Sklads.Count > 0 ? source.Sklads.Max(rec => rec.Id) : 0;
                element = new Sklad { Id = maxId + 1 };
                source.Sklads.Add(element);
            }

            element.SkladName = model.SkladName;
        }

        public void Delete(SkladBindingModel model)
        {
            source.SkladComponents.RemoveAll(rec => rec.SkladId == model.Id);
            Sklad element = source.Sklads.FirstOrDefault(rec => rec.Id == model.Id);

            if (element != null)
            {
                source.Sklads.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }

        public void AddComponent(SkladComponentBindingModel model)
        {
            Sklad warehouse = source.Sklads.FirstOrDefault(rec => rec.Id == model.SkladId);
            if (warehouse == null)
            {
                throw new Exception("Склад не найден");
            }

            Component component = source.Components.FirstOrDefault(rec => rec.Id == model.ComponentId);

            if (component == null)
            {
                throw new Exception("Компонент не найден");
            }
            SkladComponent element = source.SkladComponents
            .FirstOrDefault(rec => rec.SkladId == model.SkladId && rec.ComponentId == model.ComponentId);

            if (element != null)
            {
                element.Count += model.Count;
                return;
            }

            source.SkladComponents.Add(new SkladComponent
            {
                Id = source.SkladComponents.Count > 0 ? source.SkladComponents.Max(rec => rec.Id) + 1 : 0,
                SkladId = model.SkladId,
                ComponentId = model.ComponentId,
                Count = model.Count
            });
        }

        public List<SkladViewModel> Read(SkladBindingModel model)
        {
            return source.Sklads
            .Where(rec => model == null || rec.Id == model.Id)
            .Select(rec => new SkladViewModel
            {
                Id = rec.Id,
                SkladName = rec.SkladName,
                SkladComponents = source.SkladComponents
                .Where(recWC => recWC.SkladId == rec.Id)
                .ToDictionary(recWC => recWC.ComponentId,
                recWC => (
                source.Components.FirstOrDefault(recC => recC.Id == recWC.ComponentId)?.ComponentName, recWC.Count
                )
                )
            })
            .ToList();
        }

        public bool WriteOffComponents(RemontViewModel model)
        {
            int id = source.Ships.FirstOrDefault(rec => rec.ShipName == model.ShipName).Id;
            var product = source.Ships.FirstOrDefault(rec => rec.Id == id);
            if (product == null)
            {
                throw new Exception("Заказ не найден");
            }

            var productComponents = source.ShipComponents.Where(rec => rec.ShipId == product.Id).ToList();

            if (productComponents == null)
            {
                throw new Exception("Не найдена связь продукта с компонентами");
            }

            foreach (var pc in productComponents)
            {
                var warehouseComponent = source.SkladComponents.Where(rec => rec.ComponentId == pc.ComponentId);
                int sum = warehouseComponent.Sum(rec => rec.Count);

                if (sum < pc.Count * model.Count)
                {
                    return false;
                }
            }

            foreach (var pc in productComponents)
            {
                var warehouseComponent = source.SkladComponents.Where(rec => rec.ComponentId == pc.ComponentId);
                int neededCount = pc.Count * model.Count;
                foreach (var wc in warehouseComponent)
                {
                    if (wc.Count >= neededCount)
                    {
                        wc.Count -= neededCount;
                        break;
                    }
                    else
                    {
                        neededCount -= wc.Count;
                        wc.Count = 0;
                    }
                }
            }
            return true;
        }
    }
}
