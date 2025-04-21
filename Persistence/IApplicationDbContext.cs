using Microsoft.EntityFrameworkCore;
using Persistence.Entities;

namespace Persistence
{
    public interface IApplicationDbContext
    {
        DbSet<Customer> Customers { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<Order> Orders { get; set; }

        DbSet<Activity> Activities { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
