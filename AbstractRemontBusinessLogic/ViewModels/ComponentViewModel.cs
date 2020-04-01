using System.ComponentModel;

namespace AbstractRemontBusinessLogic.ViewModels
{
    /// <summary>     
    /// Ингредиент, требуемый для изготовления изделия 
    /// </summary> 
    public class ComponentViewModel
    {
        public int Id { get; set; }

        [DisplayName("Компонент")] 
        public string ComponentName { get; set; }
    }
}
