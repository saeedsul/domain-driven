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
using Common.Dtos.Product;

namespace TestProject.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IApplicationDbContext> _dbContextMock;
        private readonly ProductService _sut;
        private readonly Fixture _fixture;

        public ProductServiceTests()
        {
            _dbContextMock = new Mock<IApplicationDbContext>();
            _sut = new ProductService(_dbContextMock.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task CreateProduct_ValidInput_Should_ReturnsProductDto()
        {
            // Arrange
            var addProductDto = _fixture.Create<AddProductDto>();

            var _mockDbSet = new Mock<DbSet<Product>>();

            _dbContextMock.Setup(m => m.Products).Returns(_mockDbSet.Object);

            _mockDbSet.Setup(m => m.AddAsync(It.IsAny<Product>(), default))
                .Returns((Product c, CancellationToken token) =>
                {
                    c.Id = 1;
                    return new ValueTask<EntityEntry<Product>>(new Mock<EntityEntry<Product>>().Object);
                });

            // Set up SaveChangesAsync
            _dbContextMock.Setup(m => m.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _sut.CreateProduct(addProductDto);

            // Assert
            Assert.NotNull(result);

            _dbContextMock.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task CreateProduct_InvalidInput_Should_ReturnsNull()
        {
            // Arrange
            var addProductDto = new AddProductDto();

            // Act
            var result = await _sut.CreateProduct(addProductDto);

            // Assert
            Assert.Null(result);

            _dbContextMock.Verify(m => m.SaveChangesAsync(default), Times.Never);
        }


        [Fact]
        public async Task DeleteProduct_ExistingProductId_Should_ReturnsTrue()
        {
            // Arrange
            var existingProductId = 1;

            var mock = TestDataHelper.GetFakeProductList().BuildMock().BuildMockDbSet();
            mock.Setup(x => x.FindAsync(existingProductId)).ReturnsAsync(
                TestDataHelper.GetFakeProductList().Find(e => e.Id == existingProductId));

            _dbContextMock.Setup(m => m.Products).Returns(mock.Object);

            // Act
            var result = await _sut.DeleteProduct(existingProductId);

            // Assert
            Assert.True(result);

            _dbContextMock.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task DeleteProduct_NonExistingProductId_Should_ReturnsFalse()
        {
            // Arrange
            var nonExistingProductId = 999;

            var mock = TestDataHelper.GetFakeProductList().BuildMock().BuildMockDbSet();
            mock.Setup(x => x.FindAsync(nonExistingProductId)).ReturnsAsync(
                TestDataHelper.GetFakeProductList().Find(e => e.Id == nonExistingProductId));

            _dbContextMock.Setup(m => m.Products).Returns(mock.Object);

            // Act
            var result = await _sut.DeleteProduct(nonExistingProductId);

            // Assert
            Assert.False(result);

            _dbContextMock.Verify(m => m.SaveChangesAsync(default), Times.Never);
        }

        [Fact]
        public async Task GetAllProducts_Should_ReturnListOfProductDto()
        {
            // Arrange 
            var mock = TestDataHelper.GetFakeProductList().BuildMock().BuildMockDbSet();
            _dbContextMock.SetupGet(m => m.Products).Returns(mock.Object);

            // Act
            var result = await _sut.GetAllProducts();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetProductById_ExistingId_Should_ReturnProductDto()
        {
            // Arrange
            var existingProductId = 1;

            var mock = TestDataHelper.GetFakeProductList().BuildMock().BuildMockDbSet();
            mock.Setup(x => x.FindAsync(existingProductId)).ReturnsAsync(
                TestDataHelper.GetFakeProductList().Find(e => e.Id == existingProductId));

            _dbContextMock.Setup(m => m.Products).Returns(mock.Object);


            // Act
            var result = await _sut.GetProductById(existingProductId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingProductId, result.Id);
        }

        [Fact]
        public async Task GetProductById_NonExistingId_Should_ReturnNull()
        {
            // Arrange
            var nonExistingProductId = 999;

            var mock = TestDataHelper.GetFakeProductList().BuildMock().BuildMockDbSet();
            mock.Setup(x => x.FindAsync(nonExistingProductId)).ReturnsAsync(
                TestDataHelper.GetFakeProductList().Find(e => e.Id == nonExistingProductId));

            _dbContextMock.Setup(m => m.Products).Returns(mock.Object);


            // Act
            var result = await _sut.GetProductById(nonExistingProductId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateProduct_ExistingIdAndValidDto_Should_ReturnUpdatedProductDto()
        {
            // Arrange
            var existingProductId = 1;
            var updateProductDto = new UpdateProductDto { Name = "UpdatedName", Description = "Test", SKU = "test" };

            var mock = TestDataHelper.GetFakeProductList().BuildMock().BuildMockDbSet();
            mock.Setup(x => x.FindAsync(existingProductId)).ReturnsAsync(
                TestDataHelper.GetFakeProductList().Find(e => e.Id == existingProductId));

            _dbContextMock.Setup(m => m.Products).Returns(mock.Object);


            _dbContextMock.Setup(m => m.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _sut.UpdateProduct(existingProductId, updateProductDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingProductId, result.Id);
            Assert.Equal(updateProductDto.Name, result.Name);

            _dbContextMock.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task UpdateProduct_NonExistingId_Should_ReturnNull()
        {
            // Arrange
            var nonExistingProductId = 999;
            var updateProductDto = new UpdateProductDto { Name = "UpdatedName" };

            var mock = TestDataHelper.GetFakeProductList().BuildMock().BuildMockDbSet();
            mock.Setup(x => x.FindAsync(nonExistingProductId)).ReturnsAsync(
                TestDataHelper.GetFakeProductList().Find(e => e.Id == nonExistingProductId));

            _dbContextMock.Setup(m => m.Products).Returns(mock.Object);

            // Act
            var result = await _sut.UpdateProduct(nonExistingProductId, updateProductDto);

            // Assert
            Assert.Null(result);
            _dbContextMock.Verify(m => m.SaveChangesAsync(default), Times.Never);
        }
    }
}
