using System.ComponentModel;

namespace AbstractRemontBusinessLogic.ViewModels
{
    /// <summary>     
    /// Компонент, требуемый для изготовления коробля 
    /// </summary> 
    public class ComponentViewModel
    {
        public int Id { get; set; }

        [DisplayName("Компонент")] 
        public string ComponentName { get; set; }
    }
}
