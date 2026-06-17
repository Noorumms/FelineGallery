using Feline_Gallery_v1.Models;

namespace Feline_Gallery_v1.ViewModels
{
    public class CartVM
    {
        public Cart Cart { get; set; }
        public decimal ShippingCost { get; set; } = 0; // Free shipping or calculate
        public decimal Tax { get; set; } = 0; // Calculate if needed
        public decimal GrandTotal => Cart.TotalAmount + ShippingCost + Tax;
    }
}
