namespace Feline_Gallery_v1.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ArtworkId { get; set; }
        public decimal Price { get; set; }

        public Order Order { get; set; }

       
        public int Quantity { get; set; } = 1;
        public Artwork Artwork { get; set; }
    }
}
