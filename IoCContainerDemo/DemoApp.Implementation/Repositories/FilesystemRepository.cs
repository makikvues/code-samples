namespace DemoApp.Implementation.Repositories
{
    using System;
    using System.Collections.Generic;
    using DemoApp.Interfaces;
    using Newtonsoft.Json;

    // testing class with multiple constructors
    public class FilesystemRepository<T> : IRepository<T>
    {
        public FilesystemRepository()
        {
        }

        public FilesystemRepository(ILogger logger)
        {
        }

        public T FindById(int i)
        {
            // TODO
            // ... real implementation
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public T Save(T obj)
        {
            // TODO
            // real implementation
            string serializedObj = JsonConvert.SerializeObject(obj);

            Console.WriteLine($"saving: {serializedObj}");

            return obj;
        }
    }
}