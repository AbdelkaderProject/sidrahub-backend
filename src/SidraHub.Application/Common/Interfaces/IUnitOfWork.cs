using SidraHub.Domain.Common;
using SidraHub.Domain.Entities;

namespace SidraHub.Application.Common.Interfaces;

public interface IUnitOfWork
{
    IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
    IGenericRepository<ServiceCategory> ServiceCategories { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
