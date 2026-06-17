namespace Feline_Gallery_v1.Models.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetByUserIdAsync(string userId);
        Task<Order> GetWithItemsAsync(int orderId);
        Task<int> CreateOrderWithItemsAsync(Order order, List<OrderItem> items);
        Task<bool> UpdateOrderStatusAsync(int orderId, string status);
    }
}
