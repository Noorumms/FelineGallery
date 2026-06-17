using Dapper;
using Feline_Gallery_v1.Models;
using Feline_Gallery_v1.Models.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Feline_Gallery_v1.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection Connection => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            using var conn = Connection;
            return await conn.QueryAsync<Order>("SELECT * FROM Orders ORDER BY OrderDate DESC");
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            using var conn = Connection;
            return await conn.QueryFirstOrDefaultAsync<Order>("SELECT * FROM Orders WHERE OrderId = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(Order entity)
        {
            using var conn = Connection;
            var sql = @"INSERT INTO Orders (UserId, CustomerName, CustomerEmail, CustomerPhone, ShippingAddress, TotalAmount, OrderDate, Status)
                       VALUES (@UserId, @CustomerName, @CustomerEmail, @CustomerPhone, @ShippingAddress, @TotalAmount, @OrderDate, @Status);
                       SELECT CAST(SCOPE_IDENTITY() as int)";
            return await conn.ExecuteScalarAsync<int>(sql, entity);
        }

        public async Task<bool> UpdateAsync(Order entity)
        {
            using var conn = Connection;
            var sql = "UPDATE Orders SET Status = @Status, ShippingAddress = @ShippingAddress WHERE OrderId = @OrderId";
            var result = await conn.ExecuteAsync(sql, entity);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var conn = Connection;
            var result = await conn.ExecuteAsync("DELETE FROM Orders WHERE OrderId = @Id", new { Id = id });
            return result > 0;
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(string userId)
        {
            using var conn = Connection;
            return await conn.QueryAsync<Order>("SELECT * FROM Orders WHERE UserId = @UserId ORDER BY OrderDate DESC", new { UserId = userId });
        }

        public async Task<Order> GetWithItemsAsync(int orderId)
        {
            using var conn = Connection;
            var sql = @"SELECT * FROM Orders WHERE OrderId = @OrderId;
                       SELECT oi.*, a.Title, a.ImageUrl FROM OrderItems oi
                       INNER JOIN Artworks a ON oi.ArtworkId = a.ArtworkId
                       WHERE oi.OrderId = @OrderId";

            using var multi = await conn.QueryMultipleAsync(sql, new { OrderId = orderId });
            var order = await multi.ReadFirstOrDefaultAsync<Order>();
            if (order != null)
            {
                order.OrderItems = (await multi.ReadAsync<OrderItem>()).ToList();
            }
            return order;
        }

        public async Task<int> CreateOrderWithItemsAsync(Order order, List<OrderItem> items)
        {
            using var conn = Connection;
            conn.Open();
            using var transaction = conn.BeginTransaction();

            try
            {
                // Insert order
                var orderSql = @"INSERT INTO Orders (UserId, CustomerName, CustomerEmail, CustomerPhone, ShippingAddress, TotalAmount, OrderDate, Status)
                               VALUES (@UserId, @CustomerName, @CustomerEmail, @CustomerPhone, @ShippingAddress, @TotalAmount, @OrderDate, @Status);
                               SELECT CAST(SCOPE_IDENTITY() as int)";
                var orderId = await conn.ExecuteScalarAsync<int>(orderSql, order, transaction);

                // Insert order items
                var itemSql = "INSERT INTO OrderItems (OrderId, ArtworkId, Price, Quantity) VALUES (@OrderId, @ArtworkId, @Price, @Quantity)";
                foreach (var item in items)
                {
                    item.OrderId = orderId;
                    await conn.ExecuteAsync(itemSql, item, transaction);
                }

                transaction.Commit();
                return orderId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
        {
            using var conn = Connection;
            var result = await conn.ExecuteAsync("UPDATE Orders SET Status = @Status WHERE OrderId = @OrderId", new { OrderId = orderId, Status = status });
            return result > 0;
        }
    }
}