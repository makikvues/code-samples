namespace IoCContainer.Demo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// IoC container implementation Demo.
    /// </summary>
    public class SimpleContainer
    {
        private readonly Dictionary<Type, Type> _typeMap = new Dictionary<Type, Type>();
        private readonly Dictionary<Type, object> _singletonMap = new Dictionary<Type, object>();

        /// <summary>
        /// Registers destination type for source type
        /// generic strongly typed version with type inheritance check
        /// class constraint - must be reference types.
        /// </summary>
        /// <typeparam name="TSourceType">source type</typeparam>
        /// <typeparam name="TDestinationType">destination type</typeparam>
        /// <returns>container instance</returns>
        public SimpleContainer Register<TSourceType, TDestinationType>()
            where TSourceType : class
            where TDestinationType : class, TSourceType
        {
            return this.Register(typeof(TSourceType), typeof(TDestinationType));
        }

        /// <summary>
        /// Registers destination type for source type
        /// this supports open generics.
        /// </summary>
        /// <param name="sourceType">source type</param>
        /// <param name="destinationType">destination type</param>
        /// <returns>container instance</returns>
        public SimpleContainer Register(Type sourceType, Type destinationType)
        {
            this.UnRegister(sourceType, destinationType);

            this._typeMap[sourceType] = destinationType;

            return this;
        }

        /// <summary>
        /// Registers instance as singleton.
        /// </summary>
        /// <param name="instance">object instance</param>
        /// <returns>container instance</returns>
        public SimpleContainer RegisterSingleton(object instance)
        {
            Type type = instance.GetType();

            this.UnRegister(type, type);

            this._singletonMap[type] = instance;

            return this;
        }

        /// <summary>
        /// Resolves dependency for the source type,
        /// generic overload.
        /// </summary>
        /// <typeparam name="T">generic type to be resolved</typeparam>
        /// <returns>creates new instance of T type</returns>
        public T Resolve<T>()
        {
            return (T)this.Resolve(typeof(T));
        }

        private object Resolve(Type type)
        {
            if (this._singletonMap.ContainsKey(type))
            {
                return this._singletonMap[type];
            }

            if (this._typeMap.TryGetValue(type, out var destinationType))
            {
                return this.CreateInstance(destinationType);
            }

            if (!type.IsAbstract)
            {
                return this.CreateInstance(type);
            }

            if (type.IsGenericType &&
                this._typeMap.ContainsKey(type.GetGenericTypeDefinition()))
            {
                // type is generic
                // GetGenericTypeDefinition - returns unbound generic type
                Type unboundGenericType = this._typeMap[type.GetGenericTypeDefinition()];

                // creates constructed generic type
                // GenericTypeArguments - one ore more arguments defining the generic type
                Type constructedGenericType = unboundGenericType.MakeGenericType(type.GenericTypeArguments);

                return this.CreateInstance(constructedGenericType);
            }

            throw new InvalidOperationException("could not resolve type: " + type.FullName);
        }

        private object CreateInstance(Type destinationType)
        {
            var constructorParameters = destinationType
                                           .GetConstructors()
                                           .OrderByDescending(c => c.GetParameters().Count())
                                           .First()
                                           .GetParameters()
                                           .Select(param => this.Resolve(param.ParameterType))
                                           .ToArray();

            return Activator.CreateInstance(destinationType, constructorParameters);
        }

        private void UnRegister(Type sourceType, Type destinationType)
        {
            if (this._singletonMap.ContainsKey(destinationType))
            {
                this._singletonMap.Remove(destinationType);
            }

            if (this._typeMap.ContainsKey(sourceType))
            {
                this._typeMap.Remove(sourceType);
            }
        }
    }
}
