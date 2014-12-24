using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using StructureMap;

namespace PositiveTechnologies.Fibonacci.CalculationService.StructureMapDependencyResolution
{
    /// <summary>
    /// The structure map dependency scope.
    /// </summary>
    public class StructureMapDependencyScope : IDependencyScope
    {
        private readonly IContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMapDependencyScope" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public StructureMapDependencyScope(IContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            _container = container;
        }

        /// <summary>
        /// The container.
        /// </summary>
        protected IContainer Container
        {
            get { return _container; }
        }

        /// <summary>
        /// Retrieves a service from the scope.
        /// </summary>
        /// <param name="serviceType">The service to be retrieved.</param>
        /// <returns>The retrieved service.</returns>
        public object GetService(Type serviceType)
        {
            return serviceType.IsAbstract || serviceType.IsInterface
                     ? _container.TryGetInstance(serviceType)
                     : _container.GetInstance(serviceType);
        }

        /// <summary>
        /// Retrieves a collection of services from the scope.
        /// </summary>
        /// <param name="serviceType">The collection of services to be retrieved.</param>
        /// <returns>The retrieved collection of services.</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (serviceType == null) throw new ArgumentNullException("serviceType");
            return _container.GetAllInstances(serviceType).Cast<object>();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _container.Dispose();
        }
    }
}
