using Persistence.Entities;
using Persistence;
using Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Common.Dtos.Order;

namespace Services.Concrete
{
    public class OrderService : IOrderService
    {
        private readonly IApplicationDbContext _context;

        public OrderService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OrderDto?> CreateOrder(AddOrderDto addOrderDto)
        {
            if (addOrderDto == null || addOrderDto.ProductId <= 0 || addOrderDto.CustomerId <= 0
                || addOrderDto.Status == null)
                return null;

            // Check if the product and customer exist in the database
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == addOrderDto.ProductId);
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == addOrderDto.CustomerId);

            if (product == null || customer == null)
                return null;

            var newOrder = new Order
            {
                ProductId = addOrderDto.ProductId,
                CustomerId = addOrderDto.CustomerId,
                Status = addOrderDto.Status,
                CreatedDate = DateTime.UtcNow
            };

            _context.Orders.Add(newOrder);
            var rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected > 0 ? MapToOrderDto(newOrder) : null;
        }

        public async Task<bool> DeleteOrder(int id)
        {
            var orderToDelete = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);

            if (orderToDelete == null)
                return false;

            _context.Orders.Remove(orderToDelete);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrders()
        {
            var orders = await _context.Orders.ToListAsync();
            return orders.Select(MapToOrderDto);
        }

        public async Task<OrderDto?> GetOrderById(int id)
        {
            var order = await _context.Orders
                 .Include(p => p.Customer)
                 .Include(p => p.Product)
                 .AsNoTracking()
                 .SingleOrDefaultAsync(p => p.Id == id);

            return order != null ? MapToOrderDto(order) : null;
        }

        public async Task<OrderDto?> UpdateOrder(UpdateOrderDto updateOrderDto)
        {
            var existingOrder = await _context.Orders.FirstOrDefaultAsync(o => o.ProductId == updateOrderDto.ProductId && o.CustomerId == updateOrderDto.CustomerId);

            if (existingOrder == null)
                return null;

            existingOrder.Status = updateOrderDto.Status;
            existingOrder.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return MapToOrderDto(existingOrder);
        }

        private OrderDto MapToOrderDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                ProductId = order.ProductId,
                CustomerId = order.CustomerId,
                Status = order.Status,
                CreatedDate = order.CreatedDate,
                UpdatedDate = order.UpdatedDate
            };
        }
    }
}
