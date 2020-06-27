namespace AbstractRemontListImplement.Models
{
    /// <summary>
    /// Сколько компонентов требуется при изготовлении корабля
    /// </summary>
    public class ShipComponents
    {
        public int Id { get; set; }
        public int ShipId { get; set; }
        public int ComponentId { get; set; }
        public int Count { get; set; }
    }
}
