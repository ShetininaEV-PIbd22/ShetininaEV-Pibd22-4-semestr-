using System;
using System.Collections.Generic;
using System.Text;

namespace AbstractRemontBusinessLogic.BindingModels
{
    public class SkladBindingModel
    {
        public int? Id { get; set; }
        public string SkladName { get; set; }
       public Dictionary<int, (string, int)> SkladComponent { get; set; }
    }
}
