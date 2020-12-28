namespace DemoApp.Implementation.Services
{
    using DemoApp.Implementation.Models;
    using DemoApp.Interfaces;

    public class EmployeeService : IService<Employee>
    {
        public EmployeeService(IRepository<Employee> repository, ILogger logger)
        {
        }

        public Employee Create(Employee item)
        {
            return item;
        }
    }
}
