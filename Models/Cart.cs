namespace Feline_Gallery_v1.Models
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public decimal TotalAmount => Items.Sum(item => item.Price * item.Quantity);
    }
}
