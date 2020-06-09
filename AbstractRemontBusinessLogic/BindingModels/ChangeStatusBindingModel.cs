namespace AbstractRemontBusinessLogic.BindingModels
{
    /// <summary>
    /// Данные для смены статуса заказа
    /// </summary>
    public class ChangeStatusBindingModel
    {
        public int RemontId { get; set; }
        public int? ImplementerId { get; set; }
    }
}
