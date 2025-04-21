using Api.Controllers;
using AutoFixture;
using Common.Dtos.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Services.Interfaces;

namespace TestProject.Controllers
{
    public class ProductControllerTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IProductService> _mockService;
        private readonly ProductController _sut;

        public ProductControllerTests()
        {
            _fixture = new Fixture();
            _mockService = new Mock<IProductService>();
            _sut = new ProductController(_mockService.Object);
        }

        [Fact]
        public async Task CreateProduct_ValidInput_Should_ReturnsCreatedResponse()
        {
            // Arrange 
            var addProductDto = _fixture.Create<AddProductDto>();
            var expectedProductDto = _fixture.Create<ProductDto>();
            expectedProductDto.Id = 1;

            _mockService.Setup(service => service.CreateProduct(addProductDto))
                        .ReturnsAsync(expectedProductDto);

            // Act
            var result = await _sut.CreateProduct(addProductDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var productDto = Assert.IsAssignableFrom<ProductDto>(createdAtActionResult.Value);

            Assert.Equal(expectedProductDto.Id, productDto.Id);
        }

        [Fact]
        public async Task CreateCustomer_InvalidInput_Should_ReturnsBadRequest()
        {
            // Arrange 
            var addProductDto = _fixture.Create<AddProductDto>();
            addProductDto.Name = null;

            // Act
            var result = await _sut.CreateProduct(addProductDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }



        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task CreateProduct_InvalidName_Should_ReturnsBadRequest(string invalidName)
        {
            // Arrange 
            var addProductDto = _fixture.Create<AddProductDto>();
            addProductDto.Name = invalidName;

            // Act
            var result = await _sut.CreateProduct(addProductDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task GetProduct_ExistingProduct_Should_ReturnsOk()
        {
            // Arrange 
            int productId = 1;
            var expectedProductDto = _fixture.Create<ProductDto>();

            _mockService.Setup(service => service.GetProductById(productId))
                        .ReturnsAsync(expectedProductDto);

            // Act
            var result = await _sut.GetProduct(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var productDto = Assert.IsAssignableFrom<ProductDto>(okResult.Value);
            Assert.Equal(expectedProductDto, productDto);
        }

        [Fact]
        public async Task GetProduct_NonExistingProduct_Should_ReturnsNotFound()
        {
            // Arrange 
            int nonExistingProductId = 999;
            ProductDto? expectedProductDto = null;

            _mockService.Setup(service => service.GetProductById(nonExistingProductId))
                        .ReturnsAsync(expectedProductDto);

            // Act
            var result = await _sut.GetProduct(nonExistingProductId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetAllProducts_Should_ReturnsOk()
        {
            // Arrange 
            var expectedProducts = _fixture.CreateMany<ProductDto>(3).ToList();

            _mockService.Setup(service => service.GetAllProducts())
                        .ReturnsAsync(expectedProducts);

            // Act
            var result = await _sut.GetAllProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var products = Assert.IsAssignableFrom<List<ProductDto>>(okResult.Value);
            Assert.Equal(expectedProducts, products);
        }

        [Fact]
        public async Task GetAllProducts_NoProducts_Should_ReturnsOkWithEmptyList()
        {
            // Arrange 
            var expectedProducts = new List<ProductDto>();

            _mockService.Setup(service => service.GetAllProducts())
                        .ReturnsAsync(expectedProducts);

            // Act
            var result = await _sut.GetAllProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var products = Assert.IsAssignableFrom<List<ProductDto>>(okResult.Value);
            Assert.Empty(products);
        }

        [Fact]
        public async Task UpdateProduct_ExistingProduct_Should_ReturnsNoContent()
        {
            // Arrange 
            int productId = 1;
            var dto = _fixture.Create<UpdateProductDto>();
            var response = _fixture.Create<ProductDto>();

            _mockService.Setup(service => service.UpdateProduct(productId, dto))
                        .ReturnsAsync(response);

            // Act
            var result = await _sut.UpdateProduct(productId, dto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateProduct_NonExistingProduct_Should_ReturnsNotFound()
        {
            // Arrange 
            int nonExistingProductId = 999;
            var dto = _fixture.Create<UpdateProductDto>();

            _mockService.Setup(service => service.UpdateProduct(nonExistingProductId, dto))
                        .ReturnsAsync((ProductDto)null);

            // Act
            var result = await _sut.UpdateProduct(nonExistingProductId, dto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_ExistingProduct_Should_ReturnsNoContent()
        {
            // Arrange 
            int productId = 1;

            _mockService.Setup(service => service.DeleteProduct(productId))
                        .ReturnsAsync(true);

            // Act
            var result = await _sut.DeleteProduct(productId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_NonExistingProduct_Should_ReturnsNotFound()
        {
            // Arrange 
            int nonExistingProductId = 999;

            _mockService.Setup(service => service.DeleteProduct(nonExistingProductId))
                        .ReturnsAsync(false);

            // Act
            var result = await _sut.DeleteProduct(nonExistingProductId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

    }
}
