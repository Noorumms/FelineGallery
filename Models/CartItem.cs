namespace Feline_Gallery_v1.Models
{
    public class CartItem
    {

        public int ArtworkId { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string ArtistName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
