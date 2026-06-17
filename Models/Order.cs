namespace Feline_Gallery_v1.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        public ApplicationUser User { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }

        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string ShippingAddress { get; set; }

        public string Status { get; set; } = "Pending";
    }
}
