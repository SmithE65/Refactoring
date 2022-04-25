namespace Core.Data;

public interface IRepository<T> where T : EntityBase
{
    T? GetOne(int id);
    T[] GetMany(Func<IEnumerable<T>, bool> query);
    T[] GetAll();

    Task Delete(int id);
    Task Update(T data);
}
