using AbstractRemontBusinessLogic.BindingModels;
using AbstractRemontBusinessLogic.HelperModels;
using AbstractRemontBusinessLogic.Interfaces;
using AbstractRemontBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractRemontBusinessLogic.BusinessLogics
{
    public class ReportLogic
    {
        private readonly IComponentLogic componentLogic;
        private readonly IShipLogic productLogic;
        private readonly IRemontLogic orderLogic;
        public ReportLogic(IShipLogic productLogic, IComponentLogic componentLogic, IRemontLogic orderLogic)
        {
            this.productLogic = productLogic;
            this.componentLogic = componentLogic;
            this.orderLogic = orderLogic;
        }
        /// <summary>
        /// Получение списка компонент с указанием, в каких изделиях используются
        /// </summary>
        /// <returns></returns>
        public List<ReportShipComponentViewModel> GetProductComponent()
        {
            var components = componentLogic.Read(null);
            var products = productLogic.Read(null);
            var list = new List<ReportShipComponentViewModel>();
            Console.WriteLine("Ships count"+ products.Count);
            foreach (var product in products)
            {
                    foreach (var component in components)
                    {
                        if (product.ShipComponents.ContainsKey(component.Id))
                        {
                            list.Add(new ReportShipComponentViewModel
                            {
                                ShipName= product.ShipName,
                                ComponentName = component.ComponentName,
                                Count = product.ShipComponents[component.Id].Item2
                            });
                        }

                    }
            }
                return list;
        }
        /// <summary>
        /// Получение списка заказов за определенный период
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<ReportRemontsViewModel> GetOrders(ReportBindingModel model)
        {
            return orderLogic.Read(new RemontBindingModel
            {
                DateFrom = model.DateFrom,
                DateTo = model.DateTo
            })
            .Select(x => new ReportRemontsViewModel
            {
                DateCreate = x.DateCreate,
                ShipName = x.ShipName,
                Count = x.Count,
                Sum = x.Sum,
                Status = x.Status
            })
            .ToList();
        }
        /// <summary>
        /// Сохранение компонент в файл-Word
        /// </summary>
        /// <param name="model"></param>
        public void SaveProductsToWordFile(ReportBindingModel model)
        {
            SaveToWord.CreateDoc(new WordInfo
            {
                FileName = model.FileName,
                Title = "Список кораблей",
                Ships = productLogic.Read(null)
            });
        }
        /// <summary>
        /// Сохранение компонент с указаеним продуктов в файл-Excel
        /// </summary>
        /// <param name="model"></param>
        public void SaveOrdersToExcelFile(ReportBindingModel model)
        {
            //var a = GetOrders(model);

            SaveToExcel.CreateDoc(new ExcelInfo
            {
                FileName = model.FileName,
                Title = "Список заказов",
                Remonts = GetOrders(model)
            });
        }
        /// <summary>
        /// Сохранение заказов в файл-Pdf
        /// </summary>
        /// <param name="model"></param>
        public void SaveProductComponentsToPdfFile(ReportBindingModel model)
        {
            SaveToPdf.CreateDoc(new PdfInfo
            {
                FileName = model.FileName,
                Title = "Список кораблей с компонентами",
                ShipComponents = GetProductComponent()
            });
        }
    }
}
