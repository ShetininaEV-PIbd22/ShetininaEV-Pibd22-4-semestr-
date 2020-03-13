using AbstractRemontBusinessLogic.Enums;
using System;
using System.ComponentModel;

namespace AbstractRemontBusinessLogic.ViewModels
{
    /// <summary>
    /// Заявка на ремонт
    /// </summary>
    public class RemontViewModel
    {
        public int Id { get; set; }

        public int ShipId { get; set; }

        [DisplayName("Корабль")]
        public string ShipName { get; set; }

        [DisplayName("Количество")]
        public int Count { get; set; }

        [DisplayName("Сумма")]
        public decimal Sum { get; set; }

        [DisplayName("Статус")]
        public RemontStatus Status { get; set; }

        [DisplayName("Дата создания")]
        public DateTime DateCreate { get; set; }

        [DisplayName("Дата выполнения")]
        public DateTime? DateImplement { get; set; }
    }
}
