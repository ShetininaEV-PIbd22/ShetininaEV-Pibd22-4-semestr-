using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AbstractRemontBusinessLogic.ViewModels
{
    public class SkladViewModel
    {
        public int Id { get; set; }

        [DisplayName("Название склада")]
        public string SkladName { get; set; }
        public Dictionary<int, (string, int)> SkladComponents { get; set; }
    }
}
