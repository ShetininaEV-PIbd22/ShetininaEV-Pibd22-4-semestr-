namespace AbstractRemontBusinessLogic.BindingModels
{
    /// <summary>
    /// Компонент, требуемый для изготовления корабля 
    /// </summary>
    public class ComponentBindingModel
    {
        public int? Id { get; set; }
        public string ComponentName { get; set; }
    }
}