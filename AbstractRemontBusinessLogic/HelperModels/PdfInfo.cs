using AbstractRemontBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbstractRemontBusinessLogic.HelperModels
{
    class PdfInfo
    {
        public string FileName { get; set; }
        public string Title { get; set; }
        public List<ReportShipComponentViewModel> ShipComponents { get; set; }
    }
}
