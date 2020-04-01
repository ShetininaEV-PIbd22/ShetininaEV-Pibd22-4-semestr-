namespace AbstractRemontBusinessLogic.BindingModels
{
    /// <summary>
    /// Данные от клиента, для создания заказа
    /// </summary>
    public class CreateRemontBindingModel
    {
        public int ShipId { get; set; }

        public int Count { get; set; }

        public decimal Sum { get; set; }
    }
}
