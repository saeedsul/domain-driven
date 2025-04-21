using Api.Controllers;
using AutoFixture;
using Common.Dtos.Customer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Services.Interfaces;

namespace TestProject.Controllers
{
    public class CustomerControllerTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<ICustomerService> _mockService;
        private readonly CustomerController _sut;

        public CustomerControllerTests()
        {
            _fixture = new Fixture();
            _mockService = new Mock<ICustomerService>();
            _sut = new CustomerController(_mockService.Object);
        }

        [Fact]
        public async Task CreateCustomer_ValidInput_Should_ReturnsCreatedResponse()
        {
            // Arrange 
            var addCustomerDto = _fixture.Create<AddCustomerDto>();
            var expectedCustomerDto = _fixture.Create<CustomerDto>();
            expectedCustomerDto.Id = 1;

            _mockService.Setup(service => service.CreateCustomer(addCustomerDto))
                        .ReturnsAsync(expectedCustomerDto);

            // Act
            var result = await _sut.CreateCustomer(addCustomerDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var customerDto = Assert.IsAssignableFrom<CustomerDto>(createdAtActionResult.Value);

            Assert.Equal(expectedCustomerDto.Id, customerDto.Id);
        }

        [Fact]
        public async Task CreateCustomer_InvalidInput_Should_ReturnsBadRequest()
        {
            // Arrange 
            var addCustomerDto = _fixture.Create<AddCustomerDto>();
            addCustomerDto.FirstName = null;

            // Act
            var result = await _sut.CreateCustomer(addCustomerDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task CreateCustomer_InvalidFirstName_Should_ReturnsBadRequest(string invalidFirstName)
        {
            // Arrange 
            var addCustomerDto = _fixture.Create<AddCustomerDto>();
            addCustomerDto.FirstName = invalidFirstName;

            // Act
            var result = await _sut.CreateCustomer(addCustomerDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task GetCustomer_For_ExistingCustomer_Should_ReturnsOk()
        {
            // Arrange 
            int customerId = 1;
            var expectedCustomerDto = _fixture.Create<CustomerDto>();

            _mockService.Setup(service => service.GetCustomerById(customerId))
                        .ReturnsAsync(expectedCustomerDto);

            // Act
            var result = await _sut.GetCustomer(customerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var customerDto = Assert.IsAssignableFrom<CustomerDto>(okResult.Value);
            Assert.Equal(expectedCustomerDto, customerDto);
        }

        [Fact]
        public async Task GetCustomer_For_NonExistingCustomer_Should_ReturnsNotFound()
        {
            // Arrange 
            int nonExistingCustomerId = 999;
            CustomerDto? expectedCustomerDto = null;

            _mockService.Setup(service => service.GetCustomerById(nonExistingCustomerId))
                        .ReturnsAsync(expectedCustomerDto);

            // Act
            var result = await _sut.GetCustomer(nonExistingCustomerId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetAllCustomers_Should_ReturnsOk()
        {
            // Arrange 
            var expectedCustomers = _fixture.CreateMany<CustomerDto>(3).ToList();

            _mockService.Setup(service => service.GetAllCustomers())
                        .ReturnsAsync(expectedCustomers);

            // Act
            var result = await _sut.GetAllCustomers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var customers = Assert.IsAssignableFrom<List<CustomerDto>>(okResult.Value);
            Assert.Equal(expectedCustomers, customers);
        }

        [Fact]
        public async Task GetAllCustomers_NoCustomers_Should_ReturnsOkWithEmptyList()
        {
            // Arrange 
            var expectedCustomers = new List<CustomerDto>();

            _mockService.Setup(service => service.GetAllCustomers())
                        .ReturnsAsync(expectedCustomers);

            // Act
            var result = await _sut.GetAllCustomers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var customers = Assert.IsAssignableFrom<List<CustomerDto>>(okResult.Value);
            Assert.Empty(customers);
        }

        [Fact]
        public async Task UpdateCustomer_ExistingCustomer_Should_ReturnsNoContent()
        {
            // Arrange 
            int customerId = 1;
            var updateCustomerDto = _fixture.Create<UpdateCustomerDto>();
            var expectedCustomerDto = _fixture.Create<CustomerDto>();

            _mockService.Setup(service => service.UpdateCustomer(customerId, updateCustomerDto))
                        .ReturnsAsync(expectedCustomerDto);

            // Act
            var result = await _sut.UpdateCustomer(customerId, updateCustomerDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateCustomer_NonExistingCustomer_Should_ReturnsNotFound()
        {
            // Arrange 
            int nonExistingCustomerId = 999;
            var updateCustomerDto = _fixture.Create<UpdateCustomerDto>();

            _mockService.Setup(service => service.UpdateCustomer(nonExistingCustomerId, updateCustomerDto))
                        .ReturnsAsync((CustomerDto)null);

            // Act
            var result = await _sut.UpdateCustomer(nonExistingCustomerId, updateCustomerDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteCustomer_ExistingCustomer_Should_ReturnsNoContent()
        {
            // Arrange 
            int customerId = 1;

            _mockService.Setup(service => service.DeleteCustomer(customerId))
                        .ReturnsAsync(true);

            // Act
            var result = await _sut.DeleteCustomer(customerId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteCustomer_NonExistingCustomer_Should_ReturnsNotFound()
        {
            // Arrange 
            int nonExistingCustomerId = 999;

            _mockService.Setup(service => service.DeleteCustomer(nonExistingCustomerId))
                        .ReturnsAsync(false);

            // Act
            var result = await _sut.DeleteCustomer(nonExistingCustomerId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

    }
}
