using Common.Dtos.Customer;
using Persistence.Entities;
using Persistence;
using Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Services.Concrete
{
    public class CustomerService : ICustomerService
    {
        private readonly IApplicationDbContext _context;

        public CustomerService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerDto?> CreateCustomer(AddCustomerDto addCustomerDto)
        {
            if (addCustomerDto == null ||
                 string.IsNullOrEmpty(addCustomerDto.FirstName) ||
                 string.IsNullOrEmpty(addCustomerDto.LastName) ||
                 string.IsNullOrEmpty(addCustomerDto.Email) ||
                 string.IsNullOrEmpty(addCustomerDto.Phone))
                return null;

            var newCustomer = new Customer
            {
                FirstName = addCustomerDto.FirstName,
                LastName = addCustomerDto.LastName,
                Email = addCustomerDto.Email,
                Phone = addCustomerDto.Phone
            };

            _context.Customers.Add(newCustomer);
            var rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected > 0 ? MapToCustomerDto(newCustomer) : null;
        }

        public async Task<bool> DeleteCustomer(int id)
        {
            var customerToDelete = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);

            if (customerToDelete == null)
                return false;

            _context.Customers.Remove(customerToDelete);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<CustomerDto>> GetAllCustomers()
        {
            var customers = await _context.Customers.ToListAsync();
            return customers.Select(MapToCustomerDto);
        }

        public async Task<CustomerDto?> GetCustomerById(int id)
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(c => c.Id == id);
            return customer != null ? MapToCustomerDto(customer) : null;
        }

        public async Task<CustomerDto?> UpdateCustomer(int id, UpdateCustomerDto updateCustomerDto)
        {
            var existingCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);

            if (existingCustomer == null)
                return null;

            existingCustomer.FirstName = updateCustomerDto.FirstName;
            existingCustomer.LastName = updateCustomerDto.LastName;
            existingCustomer.Phone = updateCustomerDto.Phone;
            existingCustomer.Email = updateCustomerDto.Email;

            await _context.SaveChangesAsync();

            return MapToCustomerDto(existingCustomer);
        }

        private CustomerDto MapToCustomerDto(Customer customer)
        {
            return new CustomerDto
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Phone = customer.Phone
            };
        }
    }
}
