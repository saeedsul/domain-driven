using Common.Dtos.Order;

namespace Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto?> GetOrderById(int id);
        Task<IEnumerable<OrderDto>> GetAllOrders();
        Task<OrderDto?> CreateOrder(AddOrderDto addOrderDto);
        Task<OrderDto?> UpdateOrder(UpdateOrderDto updateOrderDto);
        Task<bool> DeleteOrder(int id);
    }
}
