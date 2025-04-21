using AutoFixture;
using Common.Dtos.Customer;
using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MockQueryable.Moq;
using Persistence.Entities;
using Persistence;
using Services.Concrete;
using TestProject.Helpers;
using MockQueryable;

namespace TestProject.Services
{
    public class CustomerServiceTests
    {
        private readonly Mock<IApplicationDbContext> _dbContextMock;
        private readonly CustomerService _sut;
        private readonly Fixture _fixture;

        public CustomerServiceTests()
        {
            _dbContextMock = new Mock<IApplicationDbContext>();
            _sut = new CustomerService(_dbContextMock.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task CreateCustomer_ValidInput_Should_ReturnsCustomerDto()
        {
            // Arrange
            var addCustomerDto = _fixture.Create<AddCustomerDto>();

            var _mockDbSet = new Mock<DbSet<Customer>>();

            _dbContextMock.Setup(m => m.Customers).Returns(_mockDbSet.Object);

            _mockDbSet.Setup(m => m.AddAsync(It.IsAny<Customer>(), default))
                .Returns((Customer c, CancellationToken token) =>
                {
                    c.Id = 1;
                    return new ValueTask<EntityEntry<Customer>>(new Mock<EntityEntry<Customer>>().Object);
                });

            // Set up SaveChangesAsync
            _dbContextMock.Setup(m => m.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _sut.CreateCustomer(addCustomerDto);

            // Assert
            Assert.NotNull(result);

            _dbContextMock.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task CreateCustomer_InvalidInput_Should_ReturnsNull()
        {
            // Arrange
            var addCustomerDto = new AddCustomerDto();

            // Act
            var result = await _sut.CreateCustomer(addCustomerDto);

            // Assert
            Assert.Null(result);

            _dbContextMock.Verify(m => m.SaveChangesAsync(default), Times.Never);
        }


        [Fact]
        public async Task DeleteCustomer_ExistingCustomerId_Should_ReturnsTrue()
        {
            // Arrange
            var existingCustomerId = 1;

            var mock = TestDataHelper.GetFakeCustomerList().BuildMock().BuildMockDbSet();
            mock.Setup(x => x.FindAsync(existingCustomerId)).ReturnsAsync(
                TestDataHelper.GetFakeCustomerList().Find(e => e.Id == existingCustomerId));

            _dbContextMock.Setup(m => m.Customers).Returns(mock.Object);

            // Act
            var result = await _sut.DeleteCustomer(existingCustomerId);

            // Assert
            Assert.True(result);

            _dbContextMock.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task DeleteCustomer_NonExistingCustomerId_Should_ReturnsFalse()
        {
            // Arrange
            var nonExistingCustomerId = 999;

            var mock = TestDataHelper.GetFakeCustomerList().BuildMock().BuildMockDbSet();
            mock.Setup(x => x.FindAsync(nonExistingCustomerId)).ReturnsAsync(
                TestDataHelper.GetFakeCustomerList().Find(e => e.Id == nonExistingCustomerId));

            _dbContextMock.Setup(m => m.Customers).Returns(mock.Object);

            // Act
            var result = await _sut.DeleteCustomer(nonExistingCustomerId);

            // Assert
            Assert.False(result);

            _dbContextMock.Verify(m => m.SaveChangesAsync(default), Times.Never);
        }

        [Fact]
        public async Task GetAllCustomers_Should_ReturnListOfCustomerDto()
        {
            // Arrange 
            var mock = TestDataHelper.GetFakeCustomerList().BuildMock().BuildMockDbSet();
            _dbContextMock.SetupGet(m => m.Customers).Returns(mock.Object);

            // Act
            var result = await _sut.GetAllCustomers();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetCustomerById_ExistingId_Should_ReturnCustomerDto()
        {
            // Arrange
            var existingCustomerId = 1;

            var mock = TestDataHelper.GetFakeCustomerList().BuildMock().BuildMockDbSet();
            mock.Setup(x => x.FindAsync(existingCustomerId)).ReturnsAsync(
                TestDataHelper.GetFakeCustomerList().Find(e => e.Id == existingCustomerId));

            _dbContextMock.Setup(m => m.Customers).Returns(mock.Object);


            // Act
            var result = await _sut.GetCustomerById(existingCustomerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingCustomerId, result.Id);
        }

        [Fact]
        public async Task GetCustomerById_NonExistingId_Should_ReturnNull()
        {
            // Arrange
            var nonExistingCustomerId = 999;

            var mock = TestDataHelper.GetFakeCustomerList().BuildMock().BuildMockDbSet();
            mock.Setup(x => x.FindAsync(nonExistingCustomerId)).ReturnsAsync(
                TestDataHelper.GetFakeCustomerList().Find(e => e.Id == nonExistingCustomerId));

            _dbContextMock.Setup(m => m.Customers).Returns(mock.Object);


            // Act
            var result = await _sut.GetCustomerById(nonExistingCustomerId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateCustomer_ExistingIdAndValidDto_Should_ReturnUpdatedCustomerDto()
        {
            // Arrange
            var existingCustomerId = 1;
            var updateCustomerDto = new UpdateCustomerDto { FirstName = "UpdatedName" };

            var mock = TestDataHelper.GetFakeCustomerList().BuildMock().BuildMockDbSet();
            mock.Setup(x => x.FindAsync(existingCustomerId)).ReturnsAsync(
                TestDataHelper.GetFakeCustomerList().Find(e => e.Id == existingCustomerId));

            _dbContextMock.Setup(m => m.Customers).Returns(mock.Object);


            _dbContextMock.Setup(m => m.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _sut.UpdateCustomer(existingCustomerId, updateCustomerDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingCustomerId, result.Id);
            Assert.Equal(updateCustomerDto.FirstName, result.FirstName);

            _dbContextMock.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task UpdateCustomer_NonExistingId_Should_ReturnNull()
        {
            // Arrange
            var nonExistingCustomerId = 999;
            var updateCustomerDto = new UpdateCustomerDto { FirstName = "UpdatedName" };

            var mock = TestDataHelper.GetFakeCustomerList().BuildMock().BuildMockDbSet();
            mock.Setup(x => x.FindAsync(nonExistingCustomerId)).ReturnsAsync(
                TestDataHelper.GetFakeCustomerList().Find(e => e.Id == nonExistingCustomerId));

            _dbContextMock.Setup(m => m.Customers).Returns(mock.Object);

            // Act
            var result = await _sut.UpdateCustomer(nonExistingCustomerId, updateCustomerDto);

            // Assert
            Assert.Null(result);
            _dbContextMock.Verify(m => m.SaveChangesAsync(default), Times.Never);
        }
    }
}
