using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MockQueryable.Moq;
using Persistence.Entities;
using Persistence;
using Services.Concrete;
using TestProject.Helpers;
using MockQueryable;
using Common.Dtos.Order;

namespace TestProject.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IApplicationDbContext> _dbContextMock;
        private readonly OrderService _sut;
        private readonly Fixture _fixture;

        public OrderServiceTests()
        {
            _dbContextMock = new Mock<IApplicationDbContext>();
            _sut = new OrderService(_dbContextMock.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task CreateOrder_ValidInput_Should_ReturnsOrderDto()
        {
            // Arrange

            var existingProductId = 1;

            var existingCustomerId = 1;

            var addOrderDto = _fixture.Create<AddOrderDto>();
            addOrderDto.CustomerId = existingCustomerId;
            addOrderDto.ProductId = existingProductId;

            var _mockDbSet = new Mock<DbSet<Order>>();

            _dbContextMock.Setup(m => m.Orders).Returns(_mockDbSet.Object);

            _mockDbSet.Setup(m => m.AddAsync(It.IsAny<Order>(), default))
                .Returns((Order c, CancellationToken token) =>
                {
                    c.Id = 1;
                    return new ValueTask<EntityEntry<Order>>(new Mock<EntityEntry<Order>>().Object);
                });


            var _mockProductDbSet = TestDataHelper.GetFakeProductList().BuildMock().BuildMockDbSet();
            _mockProductDbSet.Setup(x => x.FindAsync(existingProductId)).ReturnsAsync(
                TestDataHelper.GetFakeProductList().Find(e => e.Id == existingProductId));

            _dbContextMock.Setup(m => m.Products).Returns(_mockProductDbSet.Object);


            var _mockCustomerDbSet = TestDataHelper.GetFakeCustomerList().BuildMock().BuildMockDbSet();
            _mockCustomerDbSet.Setup(x => x.FindAsync(existingCustomerId)).ReturnsAsync(
                TestDataHelper.GetFakeCustomerList().Find(e => e.Id == existingCustomerId));

            _dbContextMock.Setup(m => m.Customers).Returns(_mockCustomerDbSet.Object);


            // Set up SaveChangesAsync
            _dbContextMock.Setup(m => m.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _sut.CreateOrder(addOrderDto);

            // Assert
            Assert.NotNull(result);

            _dbContextMock.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task CreateOrder_InvalidInput_Should_ReturnsNull()
        {
            // Arrange
            var addOrderDto = new AddOrderDto();

            // Act
            var result = await _sut.CreateOrder(addOrderDto);

            // Assert
            Assert.Null(result);

            _dbContextMock.Verify(m => m.SaveChangesAsync(default), Times.Never);
        }


        [Fact]
        public async Task DeleteOrder_ExistingOrderId_Should_ReturnsTrue()
        {
            // Arrange
            var existingOrderId = 1;

            var mock = TestDataHelper.GetFakeOrderList().BuildMock().BuildMockDbSet();
            mock.Setup(x => x.FindAsync(existingOrderId)).ReturnsAsync(
                TestDataHelper.GetFakeOrderList().Find(e => e.Id == existingOrderId));

            _dbContextMock.Setup(m => m.Orders).Returns(mock.Object);

            // Act
            var result = await _sut.DeleteOrder(existingOrderId);

            // Assert
            Assert.True(result);

            _dbContextMock.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task DeleteOrder_NonExistingOrderId_Should_ReturnsFalse()
        {
            // Arrange
            var nonExistingOrderId = 999;

            var mock = TestDataHelper.GetFakeOrderList().BuildMock().BuildMockDbSet();
            mock.Setup(x => x.FindAsync(nonExistingOrderId)).ReturnsAsync(
                TestDataHelper.GetFakeOrderList().Find(e => e.Id == nonExistingOrderId));

            _dbContextMock.Setup(m => m.Orders).Returns(mock.Object);

            // Act
            var result = await _sut.DeleteOrder(nonExistingOrderId);

            // Assert
            Assert.False(result);

            _dbContextMock.Verify(m => m.SaveChangesAsync(default), Times.Never);
        }

        [Fact]
        public async Task GetAllOrders_Should_ReturnListOfOrderDto()
        {
            // Arrange 
            var mock = TestDataHelper.GetFakeOrderList().BuildMock().BuildMockDbSet();
            _dbContextMock.SetupGet(m => m.Orders).Returns(mock.Object);

            // Act
            var result = await _sut.GetAllOrders();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetOrderById_ExistingId_Should_ReturnOrderDto()
        {
            // Arrange
            var existingOrderId = 1;

            var mock = TestDataHelper.GetFakeOrderList().BuildMock().BuildMockDbSet();
            mock.Setup(x => x.FindAsync(existingOrderId)).ReturnsAsync(
                TestDataHelper.GetFakeOrderList().Find(e => e.Id == existingOrderId));

            _dbContextMock.Setup(m => m.Orders).Returns(mock.Object);


            // Act
            var result = await _sut.GetOrderById(existingOrderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingOrderId, result.Id);
        }

        [Fact]
        public async Task GetOrderById_NonExistingId_Should_ReturnNull()
        {
            // Arrange
            var nonExistingOrderId = 999;

            var mock = TestDataHelper.GetFakeOrderList().BuildMock().BuildMockDbSet();
            mock.Setup(x => x.FindAsync(nonExistingOrderId)).ReturnsAsync(
                TestDataHelper.GetFakeOrderList().Find(e => e.Id == nonExistingOrderId));

            _dbContextMock.Setup(m => m.Orders).Returns(mock.Object);


            // Act
            var result = await _sut.GetOrderById(nonExistingOrderId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateOrder_ExistingIdAndValidDto_Should_ReturnUpdatedOrderDto()
        {
            // Arrange
            var existingOrderId = 1;
            var updateOrderDto = new UpdateOrderDto { ProductId = 1, CustomerId = 1, Status = "Completed" };

            var mock = TestDataHelper.GetFakeOrderList().BuildMock().BuildMockDbSet();
            mock.Setup(x => x.FindAsync(existingOrderId)).ReturnsAsync(
                TestDataHelper.GetFakeOrderList().Find(e => e.Id == existingOrderId));

            _dbContextMock.Setup(m => m.Orders).Returns(mock.Object);


            _dbContextMock.Setup(m => m.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _sut.UpdateOrder(updateOrderDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingOrderId, result.Id);
            Assert.Equal(updateOrderDto.Status, result.Status);

            _dbContextMock.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task UpdateOrder_NonExistingId_Should_ReturnNull()
        {
            // Arrange
            var nonExistingProductId = 999;
            var updateOrderDto = new UpdateOrderDto { ProductId = nonExistingProductId, CustomerId = 1, Status = "Completed" };

            var mock = TestDataHelper.GetFakeOrderList().BuildMock().BuildMockDbSet();
            mock.Setup(x => x.FindAsync(nonExistingProductId)).ReturnsAsync(
                TestDataHelper.GetFakeOrderList().Find(e => e.Id == nonExistingProductId));

            _dbContextMock.Setup(m => m.Orders).Returns(mock.Object);

            // Act
            var result = await _sut.UpdateOrder(updateOrderDto);

            // Assert
            Assert.Null(result);
            _dbContextMock.Verify(m => m.SaveChangesAsync(default), Times.Never);
        }
    }
}
