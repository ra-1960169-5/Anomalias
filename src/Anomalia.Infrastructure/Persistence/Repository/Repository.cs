using Anomalias.Application.Abstractions.Repository;
using Anomalias.Infrastructure.Persistence.Data;
using Anomalias.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Anomalias.Infrastructure.Persistence.Repository;
public abstract class Repository<TEntity, TId>(ApplicationDbContext context) : IRepository<TEntity, TId> where TEntity : Entity<TId> where TId : IEquatable<TId>
{
    protected readonly ApplicationDbContext Db = context;
    protected readonly DbSet<TEntity> Dbset = context.Set<TEntity>();

    public IUnitOfWork UnitOfWork => Db;

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
    {
        return await Dbset.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken)
    {
        return await Dbset.FindAsync([id], cancellationToken: cancellationToken);
    }

    public async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await Dbset.ToListAsync(cancellationToken);
    }

    public void Add(TEntity entity)
    {
        Dbset.Add(entity);
    }

    public void Update(TEntity entity)
    {
        Dbset.Update(entity);
    }

    public void Remove(TEntity entity)
    {
        Dbset.Remove(entity);
    }
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Db?.Dispose();
    }

}
