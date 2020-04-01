namespace AbstractRemontListImplement.Models
{
    /// <summary>
    /// Сколько компонентов требуется при изготовлении изделия
    /// </summary>
    public class ShipComponents
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int IngredientId { get; set; }
        public int Count { get; set; }
    }
}
