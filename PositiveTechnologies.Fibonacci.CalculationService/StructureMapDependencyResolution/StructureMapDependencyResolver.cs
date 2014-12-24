using System;
using System.Web.Http.Dependencies;
using StructureMap;

namespace PositiveTechnologies.Fibonacci.CalculationService.StructureMapDependencyResolution
{
    /// <summary>
    /// The structure map dependency resolver.
    /// </summary>
    /// <remarks>Allows to use Structure Map in Asp.Net applications.</remarks>
    public class StructureMapDependencyResolver : StructureMapDependencyScope, IDependencyResolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMapDependencyResolver" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public StructureMapDependencyResolver(IContainer container)
            : base(container)
        { }

        /// <summary>
        /// Starts a resolution scope.
        /// </summary>
        /// <returns>The dependency scope.</returns>
        public IDependencyScope BeginScope()
        {
            var child = Container.GetNestedContainer();
            return new StructureMapDependencyResolver(child);
        }
    }
}