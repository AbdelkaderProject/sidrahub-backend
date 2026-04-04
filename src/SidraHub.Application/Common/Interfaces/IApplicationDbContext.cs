using Microsoft.EntityFrameworkCore;
using SidraHub.Domain.Entities;

namespace SidraHub.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<ServiceCategory> ServiceCategories { get; }
    DbSet<Service> Services { get; }
    DbSet<Order> Orders { get; }
    DbSet<OrderService> OrderServices { get; }
    DbSet<Provider> Providers { get; }
    DbSet<ServiceProvider> ServiceProviders { get; }
    DbSet<Consultation> Consultations { get; }
    DbSet<Article> Articles { get; }
    DbSet<Review> Reviews { get; }
    DbSet<Notification> Notifications { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
