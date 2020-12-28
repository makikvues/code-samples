namespace IocContainer.Tests
{
    using System;
    using DemoApp.Implementation.Loggers;
    using DemoApp.Implementation.Models;
    using DemoApp.Implementation.Repositories;
    using DemoApp.Implementation.Services;
    using DemoApp.Interfaces;
    using IoCContainer.Demo;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class IoCTests
    {
        private SimpleContainer _container;

        [TestInitialize]
        public void TestInitialize()
        {
            this._container = new SimpleContainer();
        }

        [TestMethod]
        public void Should_Resolve_Type_With_Parameterless_Constructor()
        {
            // Arrange
            this._container.Register<ILogger, SqlServerLogger>();

            // Act
            var logger = this._container.Resolve<ILogger>();

            // Assert
            Assert.IsNotNull(logger);
            Assert.IsInstanceOfType(logger, typeof(SqlServerLogger));
        }

        [TestMethod]
        public void Should_Resolve_Type_Without_Default_Constructor()
        {
            // Arrange
            this._container.Register<ILogger, SqlServerLogger>();
            this._container.Register<IRepository<Employee>, SqlRepository<Employee>>();

            // Act
            var repository = this._container.Resolve<IRepository<Employee>>();

            // Assert
            Assert.IsNotNull(repository);
            Assert.IsInstanceOfType(repository, typeof(SqlRepository<Employee>));
        }

        [TestMethod]
        public void Should_Resolve_Type_With_Multiple_Constructors()
        {
            // Arrange
            this._container.Register<ILogger, SqlServerLogger>();
            this._container.Register(typeof(IRepository<>), typeof(FilesystemRepository<>));

            // Act
            var repository = this._container.Resolve<IRepository<Employee>>();

            // Assert
            Assert.IsNotNull(repository);
            Assert.IsInstanceOfType(repository, typeof(FilesystemRepository<Employee>));
        }

        [TestMethod]
        public void Should_Resolve_Concrete_Type_Different_Instances()
        {
            // Arrange
            this._container.Register<ILogger, SqlServerLogger>();

            // register unbound generics
            this._container.Register(typeof(IRepository<>), typeof(SqlRepository<>));

            // Act
            var service = this._container.Resolve<InvoiceService>();
            var service2 = this._container.Resolve<InvoiceService>();

            // Assert
            Assert.IsNotNull(service);
            Assert.IsNotNull(service2);

            Assert.IsInstanceOfType(service, typeof(InvoiceService));
            Assert.IsInstanceOfType(service2, typeof(InvoiceService));

            Assert.AreNotEqual(service, service2);
        }

        [TestMethod]
        public void Should_Resolve_Concrete_Type_Different_Implementation()
        {
            // Arrange
            this._container.Register<ILogger, SqlServerLogger>();

            // Act
            var logger = this._container.Resolve<ILogger>();

            this._container.Register<ILogger, ConsoleLogger>();
            var logger2 = this._container.Resolve<ILogger>();

            // Assert
            Assert.IsNotNull(logger);
            Assert.IsNotNull(logger2);

            Assert.IsInstanceOfType(logger, typeof(SqlServerLogger));
            Assert.IsInstanceOfType(logger2, typeof(ConsoleLogger));

            Assert.AreNotEqual(logger, logger2);
        }

        [TestMethod]
        public void Should_Resolve_Singleton_Type()
        {
            // Arrange
            this._container.RegisterSingleton(new SqlServerLogger());

            // Act
            var logger = this._container.Resolve<SqlServerLogger>();
            var logger2 = this._container.Resolve<SqlServerLogger>();

            // Assert
            Assert.IsNotNull(logger);
            Assert.IsNotNull(logger2);

            Assert.AreEqual(logger, logger2);

            Assert.IsInstanceOfType(logger2, typeof(SqlServerLogger));
        }

        [TestMethod]
        public void Should_Resolve_New_Singleton_Type()
        {
            // Arrange
            this._container.RegisterSingleton(new SqlServerLogger());

            // Act
            var logger = this._container.Resolve<SqlServerLogger>();

            this._container.RegisterSingleton(new SqlServerLogger());
            var logger2 = this._container.Resolve<SqlServerLogger>();

            // Assert
            Assert.IsNotNull(logger);
            Assert.IsNotNull(logger2);

            Assert.IsInstanceOfType(logger, typeof(SqlServerLogger));

            Assert.AreNotEqual(logger, logger2);
        }

        [TestMethod]
        public void Should_Resolve_Instance_When_Registered_As_Singleton_And_Then_Instance_Type()
        {
            // Arrange
            this._container.RegisterSingleton(new SqlServerLogger());

            // Act
            var logger = this._container.Resolve<SqlServerLogger>();

            this._container.Register<ILogger, SqlServerLogger>();
            var logger2 = this._container.Resolve<SqlServerLogger>();

            // Assert
            Assert.IsNotNull(logger);
            Assert.IsNotNull(logger2);

            Assert.IsInstanceOfType(logger, typeof(SqlServerLogger));

            Assert.AreNotEqual(logger, logger2);
        }

        [TestMethod]
        public void Should_Resolve_Singleton_When_Registered_As_Instance_And_Then_Singleton_Type()
        {
            // Arrange
            this._container.Register<ILogger, SqlServerLogger>();

            // Act
            var logger = this._container.Resolve<SqlServerLogger>();

            this._container.RegisterSingleton(new SqlServerLogger());
            var logger2 = this._container.Resolve<SqlServerLogger>();

            // Assert
            Assert.IsNotNull(logger);
            Assert.IsNotNull(logger2);

            Assert.IsInstanceOfType(logger, typeof(SqlServerLogger));

            Assert.AreNotEqual(logger, logger2);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Expect_Failure_Not_Registered()
        {
            // Act
            this._container.Resolve<InvoiceService>();
        }
    }
}
