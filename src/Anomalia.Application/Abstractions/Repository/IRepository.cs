using Anomalias.Shared;
using System.Linq.Expressions;

namespace Anomalias.Application.Abstractions.Repository;
public interface IRepository<TEntity, TId> : IDisposable where TEntity : Entity<TId> where TId : IEquatable<TId>
{
    IUnitOfWork UnitOfWork { get; }

    void Add(TEntity entity);

    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken);

    Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken);

    void Update(TEntity entity);

    void Remove(TEntity entity);

    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

}