using AbstractRemontBusinessLogic.Attributes;
using System.Collections.Generic;
using System.ComponentModel;

namespace AbstractRemontBusinessLogic.ViewModels
{
    public class ComponentViewModel : BaseViewModel
    {
        [Column(title: "Компонент", gridViewAutoSize: GridViewAutoSize.Fill)]
        public string ComponentName { get; set; }

        public override List<string> Properties() => new List<string>
        {
            "Id",
            "ComponentName"
        };
    }
}
