namespace AbstractRemontBusinessLogic.BindingModels
{
    /// <summary>
    ///  Ингредиент, требуемый для изготовления кондитерского изделия 
    /// </summary>
    public class ComponentBindingModel
    {
        public int? Id { get; set; }

        public string ComponentName { get; set; }
    }
}