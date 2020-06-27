using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AbstractRemontBusinessLogic.ViewModels
{
    public class SkladComponentViewModel
    {
        public int Id { get; set; }

        public int SkladId { get; set; }

        public int ComponentId { get; set; }

        [DisplayName("Компонент")]
        public string ComponentName { get; set; }

        [DisplayName("Количество")]
        public int Count { get; set; }
    }
}
