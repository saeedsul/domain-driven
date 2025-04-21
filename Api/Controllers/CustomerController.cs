using Common.Dtos.Customer;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : BaseController
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        [Route("create-customer")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCustomer(AddCustomerDto addCustomerDto)
        {
            var result = await _customerService.CreateCustomer(addCustomerDto);

            return result == null ? BadRequest() : CreatedAtAction(nameof(GetCustomer), new { id = result.Id }, result); // Return 400 Bad Request, otherwise return 201 Created.
        }

        [HttpGet("{id}", Name = "GetCustomer")]
        [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCustomer(int id)
        {
            var customerDto = await _customerService.GetCustomerById(id);

            return customerDto == null ? NotFound() : Ok(customerDto); // Return 404 Not Found , otherwise return 200 OK.
        }

        [HttpGet("get-all-customers", Name = "GetAlCustomers")]
        [ProducesResponseType(typeof(List<CustomerDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCustomers()
        {
            return Ok(await _customerService.GetAllCustomers());
        }

        [HttpPut("{id}", Name = "UpdateCustomer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] UpdateCustomerDto updateCustomerDto)
        {
            return await _customerService.UpdateCustomer(id, updateCustomerDto) == null ? NotFound() : NoContent(); // Return 404 Not Found, otherwise return 204 No Content.
        }

        [HttpDelete("{id}", Name = "DeleteCustomer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            return await _customerService.DeleteCustomer(id) ? NoContent() : NotFound(); // Return 404 Not Found if order is not found, otherwise return 204 No Content.
        }
    }
}
