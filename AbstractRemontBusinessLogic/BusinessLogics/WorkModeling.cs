using AbstractRemontBusinessLogic.BindingModels;
using AbstractRemontBusinessLogic.Interfaces;
using AbstractRemontBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AbstractRemontBusinessLogic.BusinessLogics
{
    public class WorkModeling
    {
        private readonly IImplementerLogic implementerLogic;
        private readonly IRemontLogic orderLogic;
        private readonly MainLogic mainLogic;
        private readonly Random rnd;

        public WorkModeling(IImplementerLogic implementerLogic, IRemontLogic orderLogic, MainLogic mainLogic)
        {
            this.implementerLogic = implementerLogic;
            this.orderLogic = orderLogic;
            this.mainLogic = mainLogic;
            rnd = new Random(1000);
        }

        /// Запуск работ
        public void DoWork()
        {
            var implementers = implementerLogic.Read(null);
            var orders = orderLogic.Read(new RemontBindingModel { FreeOrders = true });
            Console.WriteLine("work");
            foreach (var implementer in implementers)
            {
                WorkerWorkAsync(implementer, orders);
            }
        }

        /// Иммитация работы исполнителя
        private async void WorkerWorkAsync(ImplementerViewModel implementer, List<RemontViewModel> orders)
        {
            Console.WriteLine("WorkerWorkAsync");
            // ищем заказы, которые уже в работе (вдруг исполнителя прервали)
            var runOrders = await Task.Run(() => orderLogic.Read(new RemontBindingModel { ImplementerId = implementer.Id }));
            Console.WriteLine("WorkerWorkAsync1 "+runOrders);
            foreach (var order in runOrders)
            {
                Console.WriteLine(order.ImplementerFIO);
                // делаем работу заново
                Thread.Sleep(implementer.WorkingTime * rnd.Next(1, 5) * order.Count);
                mainLogic.FinishRemont(new ChangeStatusBindingModel { RemontId = order.Id });
                Console.WriteLine("do");
                // отдыхаем
                Thread.Sleep(implementer.PauseTime);
            }

            await Task.Run(() =>
            {
                foreach (var order in orders)
                {
                    // пытаемся назначить заказ на исполнителя
                    try
                    {
                        mainLogic.TakeRemontInWork(new ChangeStatusBindingModel { RemontId = order.Id, ImplementerId = implementer.Id });

                        // делаем работу
                        Thread.Sleep(implementer.WorkingTime * rnd.Next(1, 5) * order.Count);
                        mainLogic.FinishRemont(new ChangeStatusBindingModel { RemontId = order.Id });

                        // отдыхаем
                        Thread.Sleep(implementer.PauseTime);
                    }
                    catch (Exception) { }
                }
            });
        }
    }
}

