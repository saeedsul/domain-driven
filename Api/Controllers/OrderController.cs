using Common.Dtos.Order;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [Route("create-order")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrder(AddOrderDto addOrderDto)
        {
            var result = await _orderService.CreateOrder(addOrderDto);

            return result == null ? BadRequest() : CreatedAtAction(nameof(GetOrder), new { id = result.Id }, result); // Return 400 Bad Request if order is null, otherwise return 201 Created.
        }

        [HttpGet("{id}", Name = "GetOrder")]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrder(int id)
        {
            var orderDto = await _orderService.GetOrderById(id);

            return orderDto == null ? NotFound() : Ok(orderDto); // Return 404 Not Found if order is not found, otherwise return 200 OK.
        }

        [HttpGet("get-all-orders", Name = "GetAllOrders")]
        [ProducesResponseType(typeof(List<OrderDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllOrders()
        {
            return Ok(await _orderService.GetAllOrders());
        }

        [HttpPut(Name = "UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderDto updateOrderDto)
        {
            return await _orderService.UpdateOrder(updateOrderDto) == null ? NotFound() : NoContent(); // Return 404 Not Found if order is not found, otherwise return 204 No Content.
        }

        [HttpDelete("{id}", Name = "DeleteOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            return await _orderService.DeleteOrder(id) ? NoContent() : NotFound(); // Return 404 Not Found if order is not found, otherwise return 204 No Content.
        }
    }
}
