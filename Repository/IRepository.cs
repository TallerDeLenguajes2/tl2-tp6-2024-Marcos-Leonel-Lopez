public interface IRepository<T>
{
    List<T> GetAll();
    T GetById(int id);
    T Create(T obj);
    bool Remove(int id);
    T Update(T obj, int id);
}
