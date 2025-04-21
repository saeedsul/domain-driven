using Persistence.Entities;

namespace TestProject.Helpers
{
    public static class TestDataHelper
    {
        public static List<Customer> GetFakeCustomerList()
        {
            return new List<Customer>
            {
                new Customer { Id = 1, FirstName = "John" },
                new Customer { Id = 2, FirstName = "Jane" }
            };
        }

        public static List<Product> GetFakeProductList()
        {
            return new List<Product>
            {
                new Product { Id = 1, Name = "Product 1" },
                new Product { Id = 2, Name = "Product 2" }
            };
        }

        public static List<Order> GetFakeOrderList()
        {
            return new List<Order>
            {
                new Order { Id = 1, CustomerId = 1, ProductId = 1, Status="Pending" },
                new Order { Id = 2, CustomerId = 2, ProductId = 2, Status="Pending" }
            };
        }
    }
}
