using System;
using System.Collections.Generic;
using System.Text;

namespace AbstractRemontBusinessLogic.ViewModels
{
    public class ReportShipComponentViewModel
    {
        public string ComponentName { get; set; }
        public int TotalCount { get; set; }
        public List<Tuple<string, int>> Ships { get; set; }
    }
}
