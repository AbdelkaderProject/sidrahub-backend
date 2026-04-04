using System.Collections.Concurrent;
using SidraHub.Application.Common.Interfaces;
using SidraHub.Domain.Common;
using SidraHub.Domain.Entities;
using SidraHub.Infrastructure.Persistence.Repositories;

namespace SidraHub.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly SidraHubDbContext _dbContext;
    private readonly ConcurrentDictionary<Type, object> _repositories = new();
    private IGenericRepository<ServiceCategory>? _serviceCategories;

    public UnitOfWork(SidraHubDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IGenericRepository<ServiceCategory> ServiceCategories =>
        _serviceCategories ??= new GenericRepository<ServiceCategory>(_dbContext);

    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
    {
        var repository = _repositories.GetOrAdd(
            typeof(TEntity),
            _ => new GenericRepository<TEntity>(_dbContext));

        return (IGenericRepository<TEntity>)repository;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
