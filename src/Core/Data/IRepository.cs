using System.Linq.Expressions;

namespace Core.Data;

public interface IRepository<T> where T : EntityBase
{
    Task CreateAsync(T data);
    Task DeleteAsync(int id);
    Task DeleteManyAsync(IEnumerable<int> ids);
    Task<T?> GetOneAsync(int id);
    Task<T[]> GetManyAsync(Expression<Func<T, bool>> query);
    Task<T[]> GetAllAsync();
    Task SaveChangesAsync();
    Task UpdateAsync(T data);

    Task ExecuteCommandAsync<Tc>(Tc command) where Tc : IDataCommand<T>;
    Task<Tr> ExecuteQueryAsync<Tq, Tr>(Tq query) where Tq : IDataQuery<T, Tr>;
}

public interface IDataQuery<TEntity, TResult>
{
    Task<TResult> ExecuteAsync();
}

public interface IDataCommand<T>
{
    Task ExecuteAsync();
}
