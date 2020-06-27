namespace AbstractRemontListImplement.Models
{
    /// <summary>
    /// Изделие, изготавливаемое в кондитерской
    /// </summary>
    public class Ship
    {
        public int Id { get; set; }
        public string ShipName { get; set; }
        public decimal Price { get; set; }
    }
}
