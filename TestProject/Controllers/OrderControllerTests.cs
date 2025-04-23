using Api.Controllers;
using AutoFixture;
using Common.Dtos.Order;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Services.Interfaces;

namespace TestProject.Controllers
{
    public class OrderControllerTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IOrderService> _mockService;
        private readonly OrderController _sut;

        public OrderControllerTests()
        {
            _fixture = new Fixture();
            _mockService = new Mock<IOrderService>();
            _sut = new OrderController(_mockService.Object);
        }

        [Fact]
        public async Task CreateOrder_ValidInput_Should_ReturnsCreatedResponse()
        {
            // Arrange 
            var addOrderDto = _fixture.Create<AddOrderDto>();
            var expectedOrderDto = _fixture.Create<OrderDto>();

            _mockService.Setup(service => service.CreateOrder(addOrderDto))
                             .ReturnsAsync(expectedOrderDto);

            // Act
            var result = await _sut.CreateOrder(addOrderDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var orderDto = Assert.IsAssignableFrom<OrderDto>(createdAtActionResult.Value);
            Assert.Equal(expectedOrderDto, orderDto);
        }

        [Fact]
        public async Task CreateOrder_InvalidInput_Should_ReturnsBadRequest()
        {
            // Arrange 
            var addOrderDto = _fixture.Create<AddOrderDto>();
            addOrderDto.CustomerId = 0;

            // Act
            var result = await _sut.CreateOrder(addOrderDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task CreateOrder_InvalidCustomerId_Should_ReturnsBadRequest(int invalidCustomerId)
        {
            // Arrange 
            var addOrderDto = _fixture.Create<AddOrderDto>();
            addOrderDto.CustomerId = invalidCustomerId;

            // Act
            var result = await _sut.CreateOrder(addOrderDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task GetOrder_ExistingOrder_Should_ReturnsOk()
        {
            // Arrange 
            int orderId = 1;
            var expectedOrderDto = _fixture.Create<OrderDto>();

            _mockService.Setup(service => service.GetOrderById(orderId))
                             .ReturnsAsync(expectedOrderDto);

            // Act
            var result = await _sut.GetOrder(orderId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var orderDto = Assert.IsAssignableFrom<OrderDto>(okResult.Value);
            Assert.Equal(expectedOrderDto, orderDto);
        }

        [Fact]
        public async Task GetOrder_NonExistingOrder_Should_ReturnsNotFound()
        {
            // Arrange 
            int nonExistingOrderId = 999;
            OrderDto? expectedOrderDto = null;

            _mockService.Setup(service => service.GetOrderById(nonExistingOrderId))
                             .ReturnsAsync(expectedOrderDto);

            // Act
            var result = await _sut.GetOrder(nonExistingOrderId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetAllOrders_Should_ReturnsOk()
        {
            // Arrange 
            var expectedOrders = _fixture.CreateMany<OrderDto>(3).ToList();

            _mockService.Setup(service => service.GetAllOrders())
                             .ReturnsAsync(expectedOrders);

            // Act
            var result = await _sut.GetAllOrders();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var orders = Assert.IsAssignableFrom<List<OrderDto>>(okResult.Value);
            Assert.Equal(expectedOrders, orders);
        }

        [Fact]
        public async Task GetAllOrders_NoOrders_Should_ReturnsOkWithEmptyList()
        {
            // Arrange 
            var expectedOrders = new List<OrderDto>();

            _mockService.Setup(service => service.GetAllOrders())
                             .ReturnsAsync(expectedOrders);

            // Act
            var result = await _sut.GetAllOrders();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var orders = Assert.IsAssignableFrom<List<OrderDto>>(okResult.Value);
            Assert.Empty(orders);
        }

        [Fact]
        public async Task UpdateOrder_ExistingOrder_Should_ReturnsNoContent()
        {
            // Arrange 
            var updateOrderDto = _fixture.Create<UpdateOrderDto>();
            var expectedOrderDto = _fixture.Create<OrderDto>();

            _mockService.Setup(service => service.UpdateOrder(updateOrderDto))
                             .ReturnsAsync(expectedOrderDto);

            // Act
            var result = await _sut.UpdateOrder(updateOrderDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateOrder_NonExistingOrder_Should_ReturnsNotFound()
        {
            // Arrange 
            var updateOrderDto = _fixture.Create<UpdateOrderDto>(); 

            _mockService.Setup(service => service.UpdateOrder(updateOrderDto))
                             .ReturnsAsync(value: null);

            // Act
            var result = await _sut.UpdateOrder(updateOrderDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteOrder_ExistingOrder_Should_ReturnsNoContent()
        {
            // Arrange 
            int orderIdToDelete = 1;

            _mockService.Setup(service => service.DeleteOrder(orderIdToDelete))
                             .ReturnsAsync(true);

            // Act
            var result = await _sut.DeleteOrder(orderIdToDelete);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteOrder_NonExistingOrder_Should_ReturnsNotFound()
        {
            // Arrange 
            int nonExistingOrderIdToDelete = 999;

            _mockService.Setup(service => service.DeleteOrder(nonExistingOrderIdToDelete))
                             .ReturnsAsync(false);

            // Act
            var result = await _sut.DeleteOrder(nonExistingOrderIdToDelete);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

    }
}
