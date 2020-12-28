namespace DemoApp.Interfaces
{
    using System.Collections.Generic;

    public interface IRepository<T>
    {
        T Save(T obj);

        IEnumerable<T> GetAll();

        T FindById(int i);

        // etc.
    }
}
