using AbstractRemontBusinessLogic.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbstractRemontBusinessLogic.ViewModels
{
    public class ReportRemontsViewModel
    {
        public DateTime DateCreate { get; set; }
        public string ShipName { get; set; }
        public int Count { get; set; }
        public decimal Sum { get; set; }
        public RemontStatus Status { get; set; }
    }
}
