namespace DemoApp.Interfaces
{
    public interface IService<T>
    {
        T Create(T item);
    }
}
